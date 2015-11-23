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
        int userCount = 0;
        ThreadRepo clients;

        /*
        *METHOD		    :	TriviaGameDatabaseService
        *
        *DESCRIPTION	:	Constructor, start main thread and creates thread repo
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	
        *
        */
        public TriviaGameDatabaseService()
        {
            InitializeComponent();
            Done = false;
            CanPauseAndContinue = true;

            t = new Thread(new ThreadStart(listener));
            clients = new ThreadRepo();
        }

        /*
        *METHOD		    :	OnStart
        *
        *DESCRIPTION	:	Method used to start service
        *
        *PARAMETERS		:	string[] args
        *  
        *RETURNS		:	void
        *
        */
        protected override void OnStart(string[] args)
        {
            Logger.Log("Started Service");
            t.Start();//start thread
        }

        /*
        *METHOD		    :	OnStop
        *
        *DESCRIPTION	:	Method used to stop service
        *
        *PARAMETERS		:
        *  
        *RETURNS		:	void
        *
        */
        protected override void OnStop()
        {
            Done = true;
            NamedPipeClientStream client;
            StreamWriter output;

            //try connecting to server to stope blocked wait
            try
            {
                client = new NamedPipeClientStream("ServiceOutgoing");
                client.Connect(30);
                output = new StreamWriter(client);
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

        /*
        *METHOD		    :	OnPause
        *
        *DESCRIPTION	:	Method used to pause service
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        protected override void OnPause()
        {
            t.Suspend();
            Logger.Log("paused service");
        }

        /*
        *METHOD		    :	OnContinue
        *
        *DESCRIPTION	:	Method used to resume paused service
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        protected override void OnContinue()
        {
            t.Resume();
            Logger.Log("continue service");
        }

        /*
        *METHOD		    :	listener
        *
        *DESCRIPTION	:	Thread used to listen to the default named pipe
         *                  when a connection comes it creates a unique port name
         *                  starts a new thread for that connection on the new port
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void listener()
        {

            NamedPipeClientStream client;
            NamedPipeServerStream server;
            StreamReader input;
            StreamWriter output;
            string serverName = "";
            string clientPipeName = "";

            //set up pipe secutity
            Logger.Log("Waiting for connection");
            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            //start server
            server = new NamedPipeServerStream("ServiceOutgoing", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 500, 500, ps);

            while(!Done)
            {
                try
                {
                    //wait for user to connect
                    server.WaitForConnection();
                    input = new StreamReader(server);

                    //get name of computer that connect
                    serverName = input.ReadLine();
                    Logger.Log("User Connected. Computer name: " + serverName);

                    //connect back
                    client = new NamedPipeClientStream(serverName, "UserOutgoing");//add server name
                    client.Connect(30);
                    output = new StreamWriter(client);
                    Logger.Log("Connected back to user " + serverName);
                    
                    //create a unique pipe name for that computer
                    clientPipeName = "Pipe" + userCount.ToString();
                    userCount++;
                    //send the pipe name
                    output.WriteLine(clientPipeName);
                    output.Flush();

                    Logger.Log("Start Clients Thread, pipe name: " + clientPipeName);
                    //start thread to monitor that pipe
                    Thread t2 = new Thread(new ParameterizedThreadStart(clientThread));
                    t2.Start((object)clientPipeName);

                    //and the thread to the repo
                    clients.AddClient(t2, clientPipeName);
                    //disconect from that user to start listning again
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

        /*
        *METHOD		    :	clientThread
        *
        *DESCRIPTION	:	Method to listen to a spesific users pipe
         *                  Is creates the connection, listens for a command
         *                  then acts acordingly based on that command
        *
        *PARAMETERS		:	object sender:  Opject relaying information on where the even call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private async void clientThread(object clientPipeName)
        {
            string serverName = "";
            string pipeName = (string)clientPipeName;
            
            NamedPipeClientStream client;
            NamedPipeServerStream server;
            StreamReader input;
            StreamWriter output;

            //set up pipe security
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

            //get the name of the computer to connect back to
            serverName = input.ReadLine();

            //connect back
            try
            {
                client = new NamedPipeClientStream(serverName, clientPipeName + "User");
                client.Connect(30);
                output = new StreamWriter(client);

                Logger.Log("User thread connected to pipe: " + clientPipeName);

                //start loop to watch the pipe 
                while (!Done)
                {
                    //async read from pipe
                    char[] temp = new char[1023];
                    await input.ReadAsync(temp, 0, 1023);
                    //move data from pipe to a string (padded with somthing, not sure what)
                    string userCommand = new string(temp);
                    //all commands end in a period. if there is no period then the user left
                    if (!userCommand.Contains("."))
                    {
                        Logger.Log("Pipe: " + clientPipeName + "userLeft");
                        //close connection
                        output.Dispose();
                        input.Dispose();
                        //remove from repo
                        clients.DeleteClient((string)clientPipeName);
                        break;
                    }
                    //if the command is valid
                    //remove the period and all padded data
                    userCommand = userCommand.Remove(userCommand.IndexOf('.'));

                    Logger.Log("Pipe: " + clientPipeName + " Got Command: " + userCommand);
                    //select the command entered
                    if (userCommand == "NewUser")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in newUser command code");
                        //read in user name
                        string userName = input.ReadLine();
                        //add to database
                        output.WriteLine("OK");
                        output.Flush();
                        //if user not in database add
                        //set user to active
                    }
                    else if (userCommand == "GetQuestion")
                    {
                        string[] splitbuffer = new string[5];
                        string questionFromDatabase = "testQuestion|testAnswer1|testAnswer2|testAnswer3|testAnswer4|1";
                        //read in question number
                        string buffer = input.ReadLine();
                        int questionNumber = Int32.Parse(buffer);
                        Logger.Log("Pipe: " + clientPipeName + "in GetQuestion command code. Getting Question: " + questionNumber);
                        
                        //get question data
                        //put it in questionFromDatabase
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
                        //read in question number
                        string questionNumber = input.ReadLine();
                        //read in users answer
                        string answer = input.ReadLine();
                        //read in user score
                        string score = input.ReadLine();

                        //send to database
                        output.WriteLine("OK");
                        output.Flush();
                    }
                    else if (userCommand == "GameDone")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in GameDone command code");
                        string leaderboard = "temp"; ////////////////remove temp
                        //read in users score for the game
                        string gameScore = input.ReadLine();
                        //save to database
                        //get leaderboard from database
                        //send leaderboard to user
                        output.WriteLine(leaderboard);
                        output.Flush();
                    }
                    else if (userCommand == "GetLeaderboard")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in GetLeaderboard command code");
                        string leaderboard = "temp"; ////////////////remove temp
                        //get leaderboard from database
                        //send leaderboard to user
                        output.WriteLine(leaderboard);
                        output.Flush();
                    }
                    else if (userCommand == "GetCurrentStatus")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in GetCurrentStatus command code");
                        string currentStatus = "temp"; ////////////////remove temp
                        //get currentStatus from database
                        //send currentStatus to user
                        output.WriteLine(currentStatus);
                        output.Flush();
                    }
                    else if (userCommand == "SaveQuestion")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "in SaveQuestion command code");
                        int questionNumber = Int32.Parse(input.ReadLine());
                        string question = input.ReadLine();
                        string answer1 = input.ReadLine();
                        string answer2 = input.ReadLine();
                        string answer3 = input.ReadLine();
                        string answer4 = input.ReadLine();
                        int correctAnswer = Int32.Parse(input.ReadLine());
                        //send question to database

                        output.WriteLine("ok");
                        output.Flush();
                    }
                    else if (userCommand == "Quit")
                    {
                        Logger.Log("Pipe: " + clientPipeName + "Closing");
                        //close connection
                        output.Dispose();
                        input.Dispose();
                        //remove from repo
                        clients.DeleteClient((string)clientPipeName);
                        //change user to inactive
                        break;
                    }
                    else
                    {
                        Logger.Log("Pipe: " + clientPipeName + "Command not recognized");
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Pipe: " + clientPipeName + " unknown error: " + ex.Message);
                clients.DeleteClient((string)clientPipeName);
            }
        }
    }
}
