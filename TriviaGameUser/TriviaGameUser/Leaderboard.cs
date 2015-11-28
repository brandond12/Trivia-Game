/*
* FILE   : Leaderboard.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This is a form gets the leaderboard
 *              and displayes the leaderboad for the user
*/

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

        /*
        *METHOD		    :	Leaderboard
        *
        *DESCRIPTION	:	Constructor for the Leaderboard status form/class
         *                  displays the leader board to the user
        *
        *PARAMETERS		:	string leaderboard      The leaderboard recieved from the service
        *  
        *RETURNS		:	
        *
        */
        public Leaderboard(string leaderboard)
        {
            InitializeComponent();
            leaderboard = leaderboard.Replace("|", "\r\n");
            txtbx_Leaderboard.Text = leaderboard;
        }

        /*
        *METHOD		    :	btn_Close_Click
        *
        *DESCRIPTION	:	Close button to close the form
        *
        *PARAMETERS		:	object sender:  Opject relaying information on where the even call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
