/*
* FILE   : Leaderboard.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This is a form gets the leaderboard from the service
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
using System.IO;
using System.IO.Pipes;

namespace TriviaGameAdmin
{
    public partial class Leaderboard : Form
    {
        /*
        *METHOD		    :	Leaderboard
        *
        *DESCRIPTION	:	Constructor for the Leaderboard status form/class
         *                  Gets the leaderboard from the service and displays it to the form
        *
        *PARAMETERS		:	StreamReader input          reader that reads in data from the service
         *                  StreamWriter output         writer that will write to the service
        *  
        *RETURNS		:	
        *
        */
        public Leaderboard(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            //send key to the service
            output.WriteLine("GetLeaderboard.");
            output.Flush();

            //get return from service
            String leaderboard = input.ReadLine();
            //change breaks to new lines
            leaderboard = leaderboard.Replace("|", "\r\n");
            //display to text box
            txtbx_Leaderboard.Text = leaderboard;
        }
    }
}
