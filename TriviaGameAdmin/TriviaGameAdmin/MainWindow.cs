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
    public partial class MainWindow : Form
    {
        NamedPipeClientStream client;
        NamedPipeServerStream server;
        StreamReader input;
        StreamWriter output;

        public MainWindow(string userName, string serverName, string pipeName)
        {
            InitializeComponent();

            //connect to service
            server = new NamedPipeServerStream(pipeName + "User");
            server.WaitForConnection();
            input = new StreamReader(server);

            client = new NamedPipeClientStream(serverName, pipeName + "service");
            client.Connect();
            output = new StreamWriter(client);
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            output.WriteLine("SQLCommand");
            output.WriteLine(txtbx_UserInput.Text);
            txtbx_ServerOutput.Text = input.ReadLine();
        }
    }
}
