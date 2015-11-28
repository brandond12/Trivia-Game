/*
* FILE   : Leaderboard.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This is a form thats starts a thread that updates the currect status to the window
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
using System.Threading;
using System.IO;
using System.IO.Pipes;

namespace TriviaGameAdmin
{
    public partial class CurrentStatus : Form
    {
        delegate void MyCallback(Object obj);

        StreamReader input;
        StreamWriter output;
        Thread currentStatusThread;
        bool Done = false;

        /*
        *METHOD		    :	txtbxInvoke
        *
        *DESCRIPTION	:	Constructor for the Current status form/class
        *
        *PARAMETERS		:	StreamReader input          reader that reads in data from the service
         *                  StreamWriter output         writer that will write to the service
        *  
        *RETURNS		:	
        *
        */
        public CurrentStatus(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            this.input = input;
            this.output = output;
            currentStatusThread = new Thread(new ThreadStart(CurrentStatusThread));
            currentStatusThread.Start();
        }

        /*
        *METHOD		    :	CurrentStatusThread
        *
        *DESCRIPTION	:	Thread that will updat the current status
        *
        *PARAMETERS		:	void
        *  
        *RETURNS		:	void
        *
        */
        private void CurrentStatusThread()
        {
            while(!Done)
            {
                output.WriteLine("GetCurrentStatus.");
                output.Flush();
                MyCallback callback = new MyCallback(txtbxInvoke);        // Callback is instance of delegate
                try
                {
                    Invoke(callback, new object[] { input.ReadLine() });
                }
                catch (Exception)
                {
                    //form has closed
                }
                Thread.Sleep(5000);
            }
        }

        /*
        *METHOD		    :	CurrentStatus_FormClosing
        *
        *DESCRIPTION	:	triggered by the form closing, closes the thread
        *
        *PARAMETERS		:	object sender:  Object relaying information on where the event call came from
        *                   EventArgs e:    Object that contains data about the event
        * 
        *RETURNS		:	void
        *
        */
        private void CurrentStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            Done = true;
            currentStatusThread.Join();
        }

        /*
        *METHOD		    :	txtbxInvoke
        *
        *DESCRIPTION	:	This Method is used to allow threads to output to the text box
        *
        *PARAMETERS		:	Object inputLine    contains a string which will be added to the text box
        *  
        *RETURNS		:	void
        *
        */
        private void txtbxInvoke(Object inputLine)
        {
            //print to txt box
            String status = ((String)inputLine).Replace("|", "\r\n");
            txtbx_CurrectStatus.Text = status;
            //scroll to bottom og text box
            txtbx_CurrectStatus.SelectionStart = txtbx_CurrectStatus.Text.Length;
            txtbx_CurrectStatus.ScrollToCaret();
        }
    }
}
