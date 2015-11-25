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
        public CurrentStatus(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            this.input = input;
            this.output = output;
            currentStatusThread = new Thread(new ThreadStart(CurrentStatusThread));
            currentStatusThread.Start();
        }

        private void CurrentStatusThread()
        {
            while(!Done)
            {
                output.WriteLine("GetCurrentStatus.");
                output.Flush();
                MyCallback callback = new MyCallback(txtbxInvoke);        // Callback is instance of delegate
                Invoke(callback, new object[] { input.ReadLine() });
                Thread.Sleep(1000);
            }
        }

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
