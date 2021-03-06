﻿/*
* FILE   : ProgramStart.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This is a form gets the user and server name,
 *              Makes a temperary connection to the server and get what will be the dedicated pipe name
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;

namespace TriviaGameUser
{
    public partial class ProgramStart : Form
    {
        NamedPipeClientStream client;
        NamedPipeServerStream server;
        StreamReader input;
        StreamWriter output;

        string userName;
        string serverName;
        string pipeName;
        public ProgramStart()
        {
            InitializeComponent();
        }

        /*
        *METHOD		    :	btn_Submit_Click
        *
        *DESCRIPTION	:	Method called when the button it clicked.
         *                  Checks the users answer test boxes arent empty
         *                  if not, then connects to the server and gets the pipe name the program should connect to from now on.
        *
        *PARAMETERS		:	object sender:  Opject relaying information on where the even call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            if (txtbx_serverName.Text.Length > 0 && txtbx_userName.Text.Length > 0)
            {
                try
                {
                    //save the data in the text boxes
                    userName = txtbx_userName.Text;
                    serverName = txtbx_serverName.Text;
                    //set up the named pipe security
                    PipeSecurity ps = new PipeSecurity();
                    System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
                    ps.AddAccessRule(par);

                    //connect to service
                    client = new NamedPipeClientStream(serverName, "ServiceOutgoing");
                    client.Connect(100);
                    output = new StreamWriter(client);

                    //tell ther service this computers name
                    output.WriteLine(Environment.MachineName);
                    output.Flush();

                    server = new NamedPipeServerStream("UserOutgoing", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 5000, 5000, ps);
                    server.WaitForConnection();
                    input = new StreamReader(server);

                    //get namedPipe Name
                    pipeName = input.ReadLine();

                    server.Disconnect();
                    server.Dispose();
                    Close();
                }
                catch(Exception)
                {
                    MessageBox.Show("Failed to connect to Server", "Error");
                    server.Disconnect();
                    server.Dispose();
                }
            }
        }

        /*
        *METHOD		    :	getUserName
        *
        *DESCRIPTION	:	Getter for userName
        *
        *PARAMETERS		:	void
        * 
        *RETURNS		:	string userName
        *
        */
        public string getUserName()
        {
            return userName;
        }

        /*
        *METHOD		    :	getServerName
        *
        *DESCRIPTION	:	Getter for serverName
        *
        *PARAMETERS		:	void
        * 
        *RETURNS		:	string serverName
        *
        */
        public string getServerName()
        {
            return serverName;
        }

        /*
        *METHOD		    :	getPipeName
        *
        *DESCRIPTION	:	Getter for pipeName
        *
        *PARAMETERS		:	void
        * 
        *RETURNS		:	string pipeName
        *
        */
        public string getPipeName()
        {
            return pipeName;
        }
    }
}
