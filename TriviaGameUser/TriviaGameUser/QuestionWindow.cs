using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;
using System.Timers;

namespace TriviaGameUser
{
    public partial class QuestionWindow : Form
    {
        System.Timers.Timer timer; 
        NamedPipeClientStream client;
        NamedPipeServerStream server;
        StreamReader input;
        StreamWriter output;

        string userName;
        string serverName;
        string pipeName;

        int userAnswer;
        int gameScore;
        int questionScore;
        string leaderboard;

        int questionNumber;
        string question;
        string[] answers;
        int correctAnswer;

        public QuestionWindow()
        {
            InitializeComponent();
            answers = new string[4];
            questionNumber = 1;
            answers[0] = "";
            answers[1] = "";
            answers[2] = "";
            answers[3] = "";

            timer = new System.Timers.Timer();
            timer.Elapsed += on_Timer_Event;
            timer.Interval = 1000;
            timer.Start();

            //connect to service
            server = new NamedPipeServerStream(pipeName+"User");
            server.WaitForConnection();
            input = new StreamReader(server);

            client = new NamedPipeClientStream(serverName, pipeName+"service");
            client.Connect();
            output = new StreamWriter(client);
        }

        public QuestionWindow(string userName, string serverName, string pipeName)
        {
            InitializeComponent();
            this.userName = userName;
            this.serverName = serverName;
            this.pipeName = pipeName;
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            timer.Stop();
            if(rad_Answer1.Checked)
            { 
                userAnswer = 0;
            }
            else if(rad_Answer2.Checked)
            {
                userAnswer = 1;
            }
            else if(rad_Answer3.Checked)
            {
                userAnswer = 2;
            }
            else if(rad_Answer4.Checked)
            {
                userAnswer = 3;
            }
            else
            {
                userAnswer = -1;
            }

            if(userAnswer != -1)
            {
                if (userAnswer + 1 == correctAnswer)
                {
                    
                    questionScore = 1;
                    gameScore += questionScore;
                }
                sendGameAnswer();
                questionNumber++;
                if(questionNumber <= 10)
                {
                    getGameQuestion();
                    questionScore = 20;
                    timer.Start();
                }
                else
                {
                    //end game
                    sendGameScore();
                    Application.Run(new Answers(input,output));
                    Application.Run(new Leaderboard(leaderboard));
                    //clear Board
                    questionNumber = 1;
                    gameScore = 0;
                    //restart
                    getGameQuestion();
                }
            }
        }

        private void getGameQuestion()
        {
            output.WriteLine("GetQuestion");
            output.WriteLine(questionNumber);
            question = input.ReadLine();
            answers[0] = input.ReadLine();
            answers[1] = input.ReadLine();
            answers[2] = input.ReadLine();
            answers[3] = input.ReadLine();
            correctAnswer = int.Parse(input.ReadLine());

            lbl_Question.Text = question;
            lbl_answer1.Text = answers[0];
            lbl_answer2.Text = answers[1];
            lbl_answer3.Text = answers[2];
            lbl_answer4.Text = answers[3];
        }

        private void sendGameAnswer()
        {
            output.WriteLine("QuestionAnswered");
            output.WriteLine(questionNumber);
            output.WriteLine(answers[userAnswer]);
            output.WriteLine(questionScore);
            input.ReadLine();
        }

        private void sendGameScore()
        {
            output.WriteLine("GameDone");
            output.WriteLine(gameScore);
            leaderboard = input.ReadLine();
        }

        private void on_Timer_Event(Object source, ElapsedEventArgs e)
        {
            questionScore--;
            if(questionScore == 0)
            {
                timer.Stop();
            }
        }
    }
}
