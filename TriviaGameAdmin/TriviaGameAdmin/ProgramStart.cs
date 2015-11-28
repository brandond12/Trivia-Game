/*
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

namespace TriviaGameAdmin
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

        /*
        *METHOD		    :	ProgramStart
        *
        *DESCRIPTION	:	Constructor for the Edit Question form/class
        *
        *PARAMETERS		:	void
        *  
        *RETURNS		:	
        *
        */
        public ProgramStart()
        {
            InitializeComponent();
        }

        /*
        *METHOD		    :	btn_submit_Click
        *
        *DESCRIPTION	:	used to send the new question to the service when submit clicked
        *
        *PARAMETERS		:	object sender:  Object relaying information on where the event call came from
        *                   EventArgs e:    Object that contains data about the event
        * 
        *RETURNS		:	void
        *
        */
        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (txtbx_server.Text.Length > 0 && txtbx_name.Text.Length > 0)
            {
                try
                {
                    userName = txtbx_name.Text;
                    serverName = txtbx_server.Text;
                    //set up the named pipe security
                    PipeSecurity ps = new PipeSecurity();
                    System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
                    ps.AddAccessRule(par);

                    //connect to service
                    client = new NamedPipeClientStream(serverName, "ServiceOutgoing");//add server name
                    client.Connect(30);
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
                catch (Exception)
                {
                    MessageBox.Show("Failed to connect to Server", "Error");
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
