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
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                Console.WriteLine("Closing of the database connection succeeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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

                    question = Convert.ToString(myCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return question;
        }

        // Gets the answers
        public String GetAnswers(int questionNumber)
        {
            String answers = "";
            String currentAnswer = "";
            String correctAnswer;
            int correctAnswerLocation = 0;
            int counter = 1;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String answersScript = "SELECT Answer From TestAnswers WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(answersScript, sqlConnection);

                    correctAnswer = this.GetCorrectAnswer(questionNumber);

                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        currentAnswer = (String)(readData["Answer"]);
                        answers += currentAnswer + "|";
                        if (currentAnswer == correctAnswer)
                        {
                            correctAnswerLocation = counter;
                        }
                        counter++;
                    }
                    readData.Close();
                    answers += correctAnswerLocation.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                wasNameStored = false;
            }
            return wasNameStored;
        }

        // Call this after storeusername has been called
        // Enter the user into the leaderboard table
        public bool InitializeUserInLeaderboard(String userName, int gameNumber)
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
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                wasUserEntered = false;
            }
            return wasUserEntered;
        }

        // Store the user's answer
        public bool StoreUserAnswer(String userName, int gameNumber, int gameQuestion, String userAnswer, int answerScore)
        {
            bool wasAnswerStored = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String answerScript = "INSERT INTO UserAnswer (GameNumber, Name, GameQuestion, UserAnswer, AnswerScore)"
                    + "VALUES (" + gameNumber + ", '" + userName + "', " + gameQuestion + ", '" + userAnswer + "', " + answerScore + ");";

                    MySqlCommand myCommand = new MySqlCommand(answerScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    wasAnswerStored = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                wasAnswerStored = false;
            }
            return wasAnswerStored;
        }

        // users score is being passed, so alter the userGames table to change the gameScore
        public bool AlterLeaderboard(int gameNumber, String name, int gameScore)
        {
            bool isLeaderboardUpdated = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String updateScript = "UPDATE UserGames SET GameScore=" + gameScore + " WHERE Name='" + name + "';";

                    MySqlCommand myCommand = new MySqlCommand(updateScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    isLeaderboardUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                isLeaderboardUpdated = false;
            }
            return isLeaderboardUpdated;
        }

        // get the leaderboard
        public String Leaderboard(int gameNumber)
        {
            String leaderboard = "";

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String leaderboardScript = "SELECT Name, GameScore FROM UserGames WHERE GameNumber=" + gameNumber + " ORDER BY GameScore DESC;";
                    MySqlCommand myCommand = new MySqlCommand(leaderboardScript, sqlConnection);

                    MySqlDataReader readData = myCommand.ExecuteReader();

                    while (readData.Read())
                    {
                        leaderboard += readData.GetString(0) + " " + readData.GetString(1) + "\n";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                leaderboard = "";
            }
            return leaderboard;
        }


        // set the user to inactive
        public bool SetUserToInactive(String name)
        {
            bool isUserNotActive = false;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String inactiveScript = "UPDATE Users SET IsActive=FALSE WHERE Name='" + name + "';";

                    MySqlCommand myCommand = new MySqlCommand(inactiveScript, sqlConnection);
                    myCommand.ExecuteNonQuery();

                    isUserNotActive = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                isUserNotActive = false;
            }
            return isUserNotActive;
        }

        // find out if name has already been used
        public bool IsNameInDatabase(String name)
        {
            bool isNameTaken = false;
            int nameCount = 0;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String nameScript = "SELECT count(*) FROM Users WHERE Name='" + name + "';";
                    MySqlCommand myCommand = new MySqlCommand(nameScript, sqlConnection);

                    nameCount = Convert.ToInt32(myCommand.ExecuteScalar());

                    if (nameCount == 0)
                    {
                        isNameTaken = false;
                    }
                    else
                    {
                        isNameTaken = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                isNameTaken = true;
            }
            return isNameTaken;
        }

        // get the correct answer for a specific question number
        public String GetCorrectAnswer(int questionNumber)
        {
            String correctAnswer = "";

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String answerScript = "SELECT Answer FROM TestAnswers WHERE QuestionNumber=" + questionNumber + " AND IsCorrect=TRUE;";
                    MySqlCommand myCommand = new MySqlCommand(answerScript, sqlConnection);

                    correctAnswer = Convert.ToString(myCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return correctAnswer;
        }

        // edit questions and answers


        // see current status of participants


    }
}
