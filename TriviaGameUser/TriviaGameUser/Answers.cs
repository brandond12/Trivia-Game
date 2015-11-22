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

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (questionNumber == 10)
            {
                Close();
            }
            chkbx_answer1.Checked = false;
            chkbx_answer2.Checked = false;
            chkbx_answer3.Checked = false;
            chkbx_answer4.Checked = false;

            int correctAnswer;
            output.WriteLine("GetQuestion");
            output.WriteLine(questionNumber);
            lbl_gameQuestion.Text = input.ReadLine();
            chkbx_answer1.Text = input.ReadLine();
            chkbx_answer2.Text = input.ReadLine();
            chkbx_answer3.Text = input.ReadLine();
            chkbx_answer4.Text = input.ReadLine();
            correctAnswer = int.Parse(input.ReadLine());
            if(correctAnswer == 1)
            {
                chkbx_answer1.Checked = true;
            }
            else if(correctAnswer == 2)
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

            questionNumber++;
            if (questionNumber == 10)
            {
                btn_Next.Text = "Close";
            }
        }
    }
}
