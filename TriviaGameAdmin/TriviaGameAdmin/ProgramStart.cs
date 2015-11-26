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

        public ProgramStart()
        {
            InitializeComponent();
        }

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
                    client = new NamedPipeClientStream("ServiceOutgoing");//add server name
                    client.Connect(30);
                    output = new StreamWriter(client);

                    //tell ther service this computers name
                    output.WriteLine(Environment.MachineName);
                    output.Flush();

                    server = new NamedPipeServerStream("UserOutgoing", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 500, 500, ps);
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
        public string getUserName()
        {
            return userName;
        }

        public string getServerName()
        {
            return serverName;
        }

        public string getPipeName()
        {
            return pipeName;
        }
    }
}
