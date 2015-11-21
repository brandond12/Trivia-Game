using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TriviaGameAdmin
{
    public partial class ProgramStart : Form
    {
        string userName;
        string serverName;
        public ProgramStart()
        {
            InitializeComponent();
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            userName = txtbx_name.Text;
            serverName = txtbx_server.Text;
            Close();
        }
        public string getUserName()
        {
            return userName;
        }

        public string getPipeName()
        {
            return serverName;
        }
    }
}
