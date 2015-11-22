using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

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
                    String questionQuery = "SELECT Question FROM TestQuestions WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(questionQuery, sqlConnection);

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
                    String answersQuery = "SELECT Answer From TestAnswers WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(answersQuery, sqlConnection);

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
                    String nameQuery = "INSERT INTO Users (Name, IsActive) VALUES ('" + name + "', TRUE);";

                    MySqlCommand myCommand = new MySqlCommand(nameQuery, sqlConnection);
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
                    String leaderboardQuery = "INSERT INTO UserGames (GameNumber, Name, GameScore) VALUES (" + gameNumber + ", '" + userName + "', 0);";

                    MySqlCommand myCommand = new MySqlCommand(leaderboardQuery, sqlConnection);
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
                    String answerQuery = "INSERT INTO UserAnswer (GameNumber, Name, GameQuestion, UserAnswer, AnswerScore)"
                    + "VALUES (" + gameNumber + ", '" + userName + "', " + gameQuestion + ", '" + userAnswer + "', " + answerScore + ");";

                    MySqlCommand myCommand = new MySqlCommand(answerQuery, sqlConnection);
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
                    String updateQuery = "UPDATE UserGames SET GameScore=" + gameScore + " WHERE Name='" + name + "' AND GameNumber=" + gameNumber + ";";

                    MySqlCommand myCommand = new MySqlCommand(updateQuery, sqlConnection);
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
                    String leaderboardQuery = "SELECT Name, GameScore FROM UserGames WHERE GameNumber=" + gameNumber + " ORDER BY GameScore DESC;";
                    MySqlCommand myCommand = new MySqlCommand(leaderboardQuery, sqlConnection);

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
                    String inactiveQuery = "UPDATE Users SET IsActive=FALSE WHERE Name='" + name + "';";

                    MySqlCommand myCommand = new MySqlCommand(inactiveQuery, sqlConnection);
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
                    String nameQuery = "SELECT count(*) FROM Users WHERE Name='" + name + "';";
                    MySqlCommand myCommand = new MySqlCommand(nameQuery, sqlConnection);

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
                    String answerQuery = "SELECT Answer FROM TestAnswers WHERE QuestionNumber=" + questionNumber + " AND IsCorrect=TRUE;";
                    MySqlCommand myCommand = new MySqlCommand(answerQuery, sqlConnection);

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
        public String AdminQueryDatabase(String adminQuery)
        {
            String response = "";
            int columnNumber = 0;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    MySqlCommand myCommand = new MySqlCommand(adminQuery, sqlConnection);
                    MySqlDataReader readData = myCommand.ExecuteReader();

                    for (int columnCount = 0; columnCount < readData.FieldCount; columnCount++)
                    {
                        columnNumber++;
                    }

                    while (readData.Read())
                    {
                        for (int columnCount = 0; columnCount < columnNumber; columnCount++)
                        {
                            response += readData.GetString(columnCount);
                            response += " ";
                        }
                        response += "\n";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return response;
        }

        // see current status of participants ??
        public String GetCurrentStatus(int gameNumber)
        {
            String currentScores = "";

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String currentScoreQuery = "SELECT ug.Name, SUM(AnswerScore) as 'Score' FROM UserAnswer ua INNER JOIN UserGames ug" +
                    " ON ua.Name = ug.Name INNER JOIN Users u ON ug.Name = u.Name WHERE ug.GameNumber=" + gameNumber + " AND IsActive=TRUE" +
                    " GROUP BY Name ORDER BY SUM(AnswerScore) DESC;";
                    MySqlCommand myCommand = new MySqlCommand(currentScoreQuery, sqlConnection);

                    MySqlDataReader readData = myCommand.ExecuteReader();

                    while (readData.Read())
                    {
                        currentScores += readData.GetString(0) + " " + readData.GetString(1) + "\n";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                currentScores = "";
            }
            return currentScores;
        }
    }
}
