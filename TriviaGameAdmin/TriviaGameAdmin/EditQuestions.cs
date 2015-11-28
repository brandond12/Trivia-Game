/*
* FILE   : Leaderboard.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This is a form that allows the user to change questions and answers
 *              When a question is selected it auto fills the text boxes with the databases question and answer
 *              when the submit button is pressed is sends all question data to the service
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
    public partial class EditQuestions : Form
    {
        StreamReader input;
        StreamWriter output;

        /*
        *METHOD		    :	EditQuestions
        *
        *DESCRIPTION	:	Constructor for the Edit Question form/class
        *
        *PARAMETERS		:	StreamReader input          reader that reads in data from the service
         *                  StreamWriter output         writer that will write to the service
        *  
        *RETURNS		:	
        *
        */
        public EditQuestions(StreamReader input, StreamWriter output)
        {
            InitializeComponent();
            cmbbx_QuestionNumber.Items.Add("1");
            cmbbx_QuestionNumber.Items.Add("2");
            cmbbx_QuestionNumber.Items.Add("3");
            cmbbx_QuestionNumber.Items.Add("4");
            cmbbx_QuestionNumber.Items.Add("5");
            cmbbx_QuestionNumber.Items.Add("6");
            cmbbx_QuestionNumber.Items.Add("7");
            cmbbx_QuestionNumber.Items.Add("8");
            cmbbx_QuestionNumber.Items.Add("9");
            cmbbx_QuestionNumber.Items.Add("10");

            this.input = input;
            this.output = output;
        }

        /*
        *METHOD		    :	cmbbx_QuestionNumber_SelectedIndexChanged
        *
        *DESCRIPTION	:	triggered by a new question number being selected,
         *                  loads all question data into the form
        *
        *PARAMETERS		:	object sender:  Object relaying information on where the event call came from
        *                   EventArgs e:    Object that contains data about the event
        * 
        *RETURNS		:	void
        *
        */
        private void cmbbx_QuestionNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //send key word
            output.WriteLine("GetQuestion.");
            output.Flush();
            //send parameter
            output.WriteLine(cmbbx_QuestionNumber.SelectedItem);
            output.Flush();
            //read in question
            txtbx_question.Text = input.ReadLine();
            //read in answers
            txtbx_Answer1.Text = input.ReadLine();
            txtbx_Answer2.Text = input.ReadLine();
            txtbx_Answer3.Text = input.ReadLine();
            txtbx_Answer4.Text = input.ReadLine();
            //read in correct answer
            int correctAnswer = int.Parse(input.ReadLine());
            switch (correctAnswer)
            {
                case 1:
                    rdbtn_Answer1.PerformClick();
                    break;
                case 2:
                    rdbtn_Answer2.PerformClick();
                    break;
                case 3:
                    rdbtn_Answer3.PerformClick();
                    break;
                case 4:
                    rdbtn_Answer4.PerformClick();
                    break;
            }
        }

        /*
        *METHOD		    :	btn_Submit_Click
        *
        *DESCRIPTION	:	used to send the new question to the service when submit clicked
        *
        *PARAMETERS		:	object sender:  Object relaying information on where the event call came from
        *                   EventArgs e:    Object that contains data about the event
        * 
        *RETURNS		:	void
        *
        */
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            //send data to Service
            output.WriteLine("SaveQuestion.");
            output.Flush();
            output.WriteLine(cmbbx_QuestionNumber.SelectedItem);
            output.Flush();
            output.WriteLine(txtbx_question.Text);
            output.Flush();
            output.WriteLine(txtbx_Answer1.Text);
            output.Flush();
            output.WriteLine(txtbx_Answer2.Text);
            output.Flush();
            output.WriteLine(txtbx_Answer3.Text);
            output.Flush();
            output.WriteLine(txtbx_Answer4.Text);
            output.Flush();
            if (rdbtn_Answer1.Checked)
            {
                output.WriteLine("1");
            }
            else if (rdbtn_Answer2.Checked)
            {
                output.WriteLine("2");
            }
            else if (rdbtn_Answer3.Checked)
            {
                output.WriteLine("3");
            }
            else if (rdbtn_Answer4.Checked)
            {
                output.WriteLine("4");
            }
            output.Flush();
            //get back responce
            input.ReadLine();
        }
    }
}
