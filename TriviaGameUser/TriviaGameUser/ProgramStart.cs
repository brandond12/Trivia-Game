using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TriviaGameUser
{
    public partial class ProgramStart : Form
    {
        string userName;
        string serverName;
        public ProgramStart()
        {
            InitializeComponent();
        }

        public string getUserName()
        {
            return userName;
        }

        public string getPipeName()
        {
            return serverName;
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            userName = txtbx_userName.Text;
            serverName = txtbx_serverName.Text;
            Close();
        }
    }
}
