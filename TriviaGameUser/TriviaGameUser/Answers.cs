using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace TriviaGameUser
{
    public partial class Answers : Form
    {
        int questionNumber;
        StreamReader input;
        StreamWriter output;
        public Answers(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            questionNumber = 1;
            this.input = input;
            this.output = output;
            btn_Next_Click(new object(), new EventArgs());
        }

        /*
        *METHOD		    :	btn_submit_Click
        *
        *DESCRIPTION	:	Method called when the button it clicked.
         *                  Gets a question and answers from the service
         *                  puts a checkmard beside the correct answer
         *                  
        *
        *PARAMETERS		:	object sender:  Opject relaying information on where the even call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private void btn_Next_Click(object sender, EventArgs e)
        {
            int correctAnswer;
            string buffer;
            if (questionNumber == 11)
            {
                Close();
            }
            else
            {
                //set all check boxes to bot checked
                chkbx_answer1.Checked = false;
                chkbx_answer2.Checked = false;
                chkbx_answer3.Checked = false;
                chkbx_answer4.Checked = false;

                //get question and answers from service
                output.WriteLine("GetQuestion.");
                output.Flush();
                output.WriteLine(questionNumber);
                output.Flush();
                lbl_gameQuestion.Text = input.ReadLine();
                chkbx_answer1.Text = input.ReadLine();
                chkbx_answer2.Text = input.ReadLine();
                chkbx_answer3.Text = input.ReadLine();
                chkbx_answer4.Text = input.ReadLine();
                buffer = input.ReadLine();
                correctAnswer = int.Parse(buffer);

                //check the box for the correct answer
                if (correctAnswer == 1)
                {
                    chkbx_answer1.Checked = true;
                }
                else if (correctAnswer == 2)
                {
                    chkbx_answer2.Checked = true;
                }
                else if (correctAnswer == 3)
                {
                    chkbx_answer3.Checked = true;
                }
                else
                {
                    chkbx_answer4.Checked = true;
                }

                //move to next question
                questionNumber++;
                if (questionNumber == 11)
                {
                    btn_Next.Text = "Close";
                }
            }
            Thread.Sleep(200);
        }
    }
}
