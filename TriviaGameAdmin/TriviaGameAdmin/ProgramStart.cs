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
                userName = txtbx_name.Text;
                serverName = txtbx_server.Text;
                //connect to service
                server = new NamedPipeServerStream("ServiceOutgoing");
                server.WaitForConnection();
                input = new StreamReader(server);

                client = new NamedPipeClientStream(serverName, "UserOutgoing");
                client.Connect();
                output = new StreamWriter(client);

                //get namedPipe Name
                output.WriteLine("UserName"); 
                output.WriteLine(userName);
                pipeName = input.ReadLine();

                Close();
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
