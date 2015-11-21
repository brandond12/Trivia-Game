using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TriviaGameUser
{
    public partial class QuestionWindow : Form
    {
        int questionNumber;
        public QuestionWindow()
        {
            InitializeComponent();
        }

        public QuestionWindow(string userName, string pipeName)
        {
            InitializeComponent();
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}
