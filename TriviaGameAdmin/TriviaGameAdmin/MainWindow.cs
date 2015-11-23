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

            //set up the named pipe security
            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            //connect to service
            client = new NamedPipeClientStream(pipeName + "service");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            client.Connect();
            output = new StreamWriter(client);

            //tell service the name of the computer to connect back to
            output.WriteLine(Environment.MachineName);
            output.Flush();

            server = new NamedPipeServerStream(pipeName + "User");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            server.WaitForConnection();
            input = new StreamReader(server);
        }

        private void btn_EditQuestion_Click(object sender, EventArgs e)
        {
            EditQuestions edit = new EditQuestions(input, output);
            edit.Show();
        }

        private void btn_CurrentStatus_Click(object sender, EventArgs e)
        {
            CurrentStatus status = new CurrentStatus(input, output);
            status.Show();
        }

        private void btn_Leaderboard_Click(object sender, EventArgs e)
        {
            Leaderboard leaders = new Leaderboard(input, output);
            leaders.Show();
        }

        private void btn_ExportExcel_Click(object sender, EventArgs e)
        {

        }
    }
}
