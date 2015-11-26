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
using System.Threading;

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

        /*
        *METHOD		    :	QuestionWindow
        *
        *DESCRIPTION	:	Constructor for the QuestionWindow class. takes in users name, the computers name to connect to and name of the pipe to use.
         *                  Opens connestion to the service and starts the first qestion
        *
        *PARAMETERS		:	string userName     The name of the user
         *                  string serverName   The name of the computer running the server
         *                  string pipeName     The name of the por to connect to
        *  
        *RETURNS		:	
        *
        */
        public QuestionWindow(string userName, string serverName, string pipeName)
        {
            InitializeComponent();
            this.userName = userName;
            this.serverName = serverName;
            this.pipeName = pipeName;

            answers = new string[4];
            questionNumber = 1;
            answers[0] = "";
            answers[1] = "";
            answers[2] = "";
            answers[3] = "";

            //start timer for counting down users score (1 per second)
            timer = new System.Timers.Timer();
            timer.Elapsed += on_Timer_Event;
            timer.Interval = 1000;

            //connect to service
            client = new NamedPipeClientStream(pipeName + "service");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            client.Connect(30);
            output = new StreamWriter(client);

            //tell service the name of the computer to connect back to
            output.WriteLine(Environment.MachineName);
            output.Flush();

            server = new NamedPipeServerStream(pipeName + "User");//naming convention for pipe is given name(pipeName) and who has the server (service or user)
            server.WaitForConnection();
            input = new StreamReader(server);

            Thread.Sleep(100);//allow server time complete actions
            //tell service name of new user
            newUser();
            Thread.Sleep(200);//allow server time complete actions
            //get the first question
            getGameQuestion();
            //start score counter
            timer.Start();
        }

        /*
        *METHOD		    :	btn_submit_Click
        *
        *DESCRIPTION	:	Method called when the button it clicked.
         *                  Checks the users answer, gets user score for the question
         *                  Sends score to server
         *                  If there are more questions, it gets the next question
         *                  If there are no more questions is shows the user end game stats and restarts the game
        *
        *PARAMETERS		:	object sender:  Opject relaying information on where the even call came from
        *                   EventArgs e:    Object that contains data about the event
        *  
        *RETURNS		:	void
        *
        */
        private void btn_submit_Click(object sender, EventArgs e)
        {
            //question answered. stop timer
            timer.Stop();
            //check what their answer was
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

            //if answer was correct add score to game score
            if (userAnswer + 1 == correctAnswer)
            {
                gameScore += questionScore;
            }
            //if answer wrong set score to 0
            else
            {
                questionScore = 0;
            }
            //send service the users answer
            sendGameAnswer();
            Thread.Sleep(100);//allow server time complete actions

            //move to next question
            questionNumber++;
            if(questionNumber <= 10)//if more questions in test
            {
                getGameQuestion();
                Thread.Sleep(100);//allow server time complete actions
                timer.Start();//start score cound down
            }
            else//no more questions in test
            {
                //end game
                //send service final score
                sendGameScore();
                Thread.Sleep(200);//allow server time complete actions
                //show user all correct answers
                Answers answersForm = new Answers(input, output);
                answersForm.Show();
                //Get the leaderboard

                //show user the leaderboard
                Leaderboard leaderboardForm = new Leaderboard(leaderboard);
                leaderboardForm.Show();

                //clear Board
                questionNumber = 1;
                gameScore = 0;
                //restart
                getGameQuestion();
            }
            Thread.Sleep(100);
        }

        /*
        *METHOD		    :	newUser
        *
        *DESCRIPTION	:	This method sends the users name to the server
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void newUser()
        {
            //send key word
            output.WriteLine("NewUser.");
            output.Flush();
            //send parameter
            output.WriteLine(userName);
            output.Flush();
            Thread.Sleep(10);
            input.ReadLine();
        }

        /*
        *METHOD		    :	getGameQuestion
        *
        *DESCRIPTION	:	This method gets a question from the server
         *                  Parses the data out
         *                  sets the GUI to the new question
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void getGameQuestion()
        {
            questionScore = 20;
            //send key word
            output.WriteLine("GetQuestion.");
            output.Flush();
            //send parameter
            output.WriteLine(questionNumber);
            output.Flush();
            //read in question
            question = input.ReadLine();
            //read in answers
            answers[0] = input.ReadLine();
            answers[1] = input.ReadLine();
            answers[2] = input.ReadLine();
            answers[3] = input.ReadLine();
            //read in correct answer
            correctAnswer = int.Parse(input.ReadLine());

            //display new question to user
            lbl_Question.Text = question;
            lbl_answer1.Text = answers[0];
            lbl_answer2.Text = answers[1];
            lbl_answer3.Text = answers[2];
            lbl_answer4.Text = answers[3];
        }

        /*
        *METHOD		    :	sendGameAnswer
        *
        *DESCRIPTION	:	This method sends the server information about the users last answer
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void sendGameAnswer()
        {
            //send key word
            output.WriteLine("QuestionAnswered.");
            output.Flush();
            //send parameter
            output.WriteLine(questionNumber);
            output.Flush();
            //send parameter
            output.WriteLine(answers[userAnswer]);
            output.Flush();
            //send parameter
            output.WriteLine(questionScore);
            output.Flush();
            input.ReadLine();
        }

        /*
        *METHOD		    :	sendGameScore
        *
        *DESCRIPTION	:	This method is called after the end of a game
         *                  It send the server the game total score
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void sendGameScore()
        {
            //send key word
            output.WriteLine("GameDone.");
            output.Flush();
            //send parameter
            output.WriteLine(gameScore);
            output.Flush();
            leaderboard = input.ReadLine();
        }

        /*
        *METHOD		    :	on_Timer_Event
        *
        *DESCRIPTION	:	This method is called by the timer every second
         *                  It counts down the users score from 20 to 0
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        private void on_Timer_Event(Object source, ElapsedEventArgs e)
        {
            //take one point off users possible score
            questionScore--;
            //if user is at 0 points stop the timer
            if(questionScore == 0)
            {
                timer.Stop();
            }
        }

        private void lbl_answer1_Click(object sender, EventArgs e)
        {
            rad_Answer1.PerformClick();
        }

        private void lbl_answer2_Click(object sender, EventArgs e)
        {
            rad_Answer2.PerformClick();
        }

        private void lbl_answer3_Click(object sender, EventArgs e)
        {
            rad_Answer3.PerformClick();
        }

        private void lbl_answer4_Click(object sender, EventArgs e)
        {
            rad_Answer4.PerformClick();
        }
    }
}
