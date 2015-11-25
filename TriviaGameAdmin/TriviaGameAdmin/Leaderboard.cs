using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;

namespace TriviaGameAdmin
{
    public partial class Leaderboard : Form
    {
        public Leaderboard(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            output.WriteLine("GetLeaderboard.");
            output.Flush();

            String leaderboard = input.ReadLine();
            leaderboard = leaderboard.Replace("|", "\r\n");
            txtbx_Leaderboard.Text = leaderboard;
        }
    }
}
