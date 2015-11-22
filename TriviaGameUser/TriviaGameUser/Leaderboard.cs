using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriviaGameUser
{
    public partial class Leaderboard : Form
    {
        public Leaderboard()
        {
            InitializeComponent();
        }

        public Leaderboard(string leaderboard)
        {
            InitializeComponent();
            txtbx_Leaderboard.Text = leaderboard;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
