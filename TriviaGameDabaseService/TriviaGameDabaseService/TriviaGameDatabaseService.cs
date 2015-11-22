using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Pipes;

namespace TriviaGameDabaseService
{
    public partial class TriviaGameDatabaseService : ServiceBase
        {

        Thread t;
        bool Done;
        public TriviaGameDatabaseService()
        {
            InitializeComponent();
            Done = false;
            CanPauseAndContinue = true;

            t = new Thread(new ThreadStart(listener));
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("Started Service");
            t.Start();
        }

        protected override void OnStop()
        {
            Done = true;
            NamedPipeClientStream client;
            StreamWriter output;

            try
            {
                Logger.Log("connecting back to server to unblock program");
                client = new NamedPipeClientStream("ServiceOutgoing");
                client.Connect(30);
                output = new StreamWriter(client);
                Logger.Log("sending blank empty string");
                output.WriteLine("empty");
                output.Flush();
            }
            catch(Exception)
            {
                Logger.Log("thread already stoped, dont need to send message");
            }
            t.Join();
            Logger.Log("stopped service");
        }

        protected override void OnPause()
        {
            t.Suspend();
            Logger.Log("paused service");
        }

        protected override void OnContinue()
        {
            t.Resume();
            Logger.Log("continue service");
        }

        private void listener()
        {
            ThreadRepo clients = new ThreadRepo();
            NamedPipeClientStream client;
            NamedPipeServerStream server;
            StreamReader input;
            StreamWriter output;
            string serverName = "";
            string clientPipeName = "";

            Logger.Log("Waiting for connection");
            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            server = new NamedPipeServerStream("ServiceOutgoing", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 500, 500, ps);

            while(!Done)
            {
                try
                {
                    //wait for user to connect
                    server.WaitForConnection();
                    input = new StreamReader(server);
                    Logger.Log("User Connected to server");

                    serverName = input.ReadLine();
                    Logger.Log("User Computer name: " + serverName);

                    //connect back
                    client = new NamedPipeClientStream("UserOutgoing");//add server name
                    client.Connect(30);
                    output = new StreamWriter(client);
                    Logger.Log("Connected to user server");
            
                    clientPipeName = "randomPipeName";
                    output.WriteLine(clientPipeName);
                    output.Flush();

                    Logger.Log("Start Clients Thread, pipe name: " + clientPipeName);
                    Thread t2 = new Thread(new ParameterizedThreadStart(clientThread));
                    t2.Start((object)clientPipeName);

                    clients.AddClient(t2, clientPipeName);
                    server.Disconnect();
                }
                catch (IOException)
                {
                    Logger.Log("Connect() failed");
                    Done = true;
                }
                catch (Exception ex)
                {
                    Logger.Log("Unknown Error: " + ex.Message);
                    Done = true;
                }
            }

            Logger.Log("Main loop closed");
        }

        private async void clientThread(object clientPipeName)
        {
            string serverName = "";
            string pipeName = (string)clientPipeName;
            
            NamedPipeClientStream client;
            NamedPipeServerStream server;
            StreamReader input;
            StreamWriter output;

            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            Logger.Log("Client thread started");

            //wait for user to connect
            server = new NamedPipeServerStream(clientPipeName + "service", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 500, 500, ps);
            server.WaitForConnection();
            input = new StreamReader(server);

            Logger.Log("Client connected to pipe: " + clientPipeName);

            serverName = input.ReadLine();

            //connect back
            try
            {
                client = new NamedPipeClientStream(clientPipeName + "User");
                client.Connect(30);
                output = new StreamWriter(client);

                Logger.Log("User thread connected to pipe: " + clientPipeName);

                while (!Done)
                {
                    char[] temp = new char[1023];
                    await input.ReadAsync(temp, 0, 1023);
                    string userCommand = new string(temp);
                    if (!userCommand.Contains("."))
                    {
                        Logger.Log("Pipe: " + clientPipeName + "userLeft");
                        break;
                    }
                    userCommand = userCommand.Remove(userCommand.IndexOf('.'));

                    Logger.Log("Pipe: " + clientPipeName + "Length: " + userCommand.Length + " Got Command: " + userCommand);
                    if (userCommand == "NewUser")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in newUser command code");
                        string userName = input.ReadLine();
                        //add to database
                        output.WriteLine("OK");
                        output.Flush();
                    }
                    else if (userCommand == "GetQuestion")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in GetQuestion command code");
                        string[] splitbuffer = new string[5];
                        string questionFromDatabase = "testQuestion|testAnswer1|testAnswer2|testAnswer3|testAnswer4|1";
                        string buffer = input.ReadLine();
                        int questionNumber = Int32.Parse(buffer);
                        //get question data

                        splitbuffer = questionFromDatabase.Split('|');
                        output.WriteLine(splitbuffer[0]);//question
                        output.Flush();
                        output.WriteLine(splitbuffer[1]);//answer1
                        output.Flush();
                        output.WriteLine(splitbuffer[2]);//answer2
                        output.Flush();
                        output.WriteLine(splitbuffer[3]);//answer3
                        output.Flush();
                        output.WriteLine(splitbuffer[4]);//answer4
                        output.Flush();
                        output.WriteLine(splitbuffer[5]);//correct answer
                        output.Flush();
                    }
                    else if (userCommand == "QuestionAnswered")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in QuestionAnswered command code");
                        string questionNumber = input.ReadLine();
                        string answer = input.ReadLine();
                        string score = input.ReadLine();

                        //send to database
                        output.WriteLine("OK");
                        output.Flush();
                    }
                    else if (userCommand == "GameDone")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in GameDone command code");
                        string leaderboard = "temp"; ////////////////remove temp
                        string gameScore = input.ReadLine();
                        //save to database
                        //get leaderboard from database
                        output.WriteLine(leaderboard);
                        output.Flush();
                    }
                    else if (userCommand == "Quit")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "Closing");
                        break;
                    }
                }
            }
            catch(Exception)
            {
                Logger.Log("Pipe: " + clientPipeName + "unknown error");

            }
        }
    }
}
