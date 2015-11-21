using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace TriviaGameDabaseService
{
    // Writing to the console for testing purposes...
    class DAL
    {
        private MySqlConnection sqlConnection;

        public DAL()
        {
            // How the database is accessed shouldn't be hardcoded, but for now it shall be
            String server = "localhost";
            String database = "TriviaGame";
            String uID = "root";
            String password = "root";

            String connectionString = "server=" + server + ";" +
                                      "database=" + database + ";" +
                                      "uid=" + uID + ";" +
                                      "pwd=" + password + ";";

            sqlConnection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Connection to database succeeded.");
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }

        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                Console.WriteLine("Closing of the database connection succeeded.");
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }

        // Gets the question
        public String GetQuestion(int questionNumber)
        {
            String question = "";

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String questionScript = "SELECT Question FROM TestQuestions WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(questionScript, sqlConnection);

                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        question = (String)(readData["Question"]);
                    }
                    readData.Close();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
            return question;
        }

        // Gets the answers
        public String GetAnswers(int questionNumber)
        {
            String answers = "";
            int counter = 1;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String answersScript = "SELECT Answer From TestAnswers WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(answersScript, sqlConnection);

                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        answers += counter + ". " + ((String) (readData["Answer"]));
                        answers += "\n";
                        counter++;
                    }
                    readData.Close();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
            return answers;
        }

        // Store the user's name in the database
        public bool StoreUserName(String name)
        {
            bool wasNameStored = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String nameScript = "INSERT INTO Users (Name, IsActive) VALUES ('" + name + "', TRUE);";

                    MySqlCommand myCommand = new MySqlCommand(nameScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    wasNameStored = true;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                wasNameStored = false;
            }
            return wasNameStored;
        }

        // Call this after storeusername has been called
        // Enter the user into the leaderboard table
        public bool EnterUserInLeaderboard(String userName, int gameNumber)
        {
            bool wasUserEntered = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String leaderboardScript = "INSERT INTO UserGames (GameNumber, Name, GameScore) VALUES (" + gameNumber + ", '" + userName + "', 0);";

                    MySqlCommand myCommand = new MySqlCommand(leaderboardScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    wasUserEntered = true;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                wasUserEntered = false;
            }
            return wasUserEntered;
        }

        public bool StoreUserAnswer(String userName, int gameNumber, int gameQuestion, String userAnswer, int answerScore)
        {
            bool wasAnswerStored = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    // check if answer was correct - make a function?

                    String answerScript = "INSERT INTO UserAnswer (GameNumber, Name, GameQuestion, UserAnswer, AnswerScore)"
                    + "VALUES (" + gameNumber + ", '" + userName + "', " + gameQuestion + ", '" + userAnswer + "', " + answerScore + ");";

                    MySqlCommand myCommand = new MySqlCommand(answerScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    wasAnswerStored = true;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
                wasAnswerStored = false;
            }
            return wasAnswerStored;
        }



    }
}
