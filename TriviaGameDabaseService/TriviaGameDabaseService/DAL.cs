using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace TriviaGameDabaseService
{
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
                Logger.Log("Connection to database succeeded.");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                Logger.Log("Closing of the database connection succeeded.");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
            }
            return question;
        }

        public String GetQuestionAndAnswers(int questionNumber)
        {
            String response = "";

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    response += this.GetQuestion(questionNumber) + "|";
                    response += this.GetAnswers(questionNumber);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return response;
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                        leaderboard += readData.GetString(0) + " " + readData.GetString(1) + "|";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                Logger.Log("Exception: " + ex.Message);
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
                        response += "|";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return response;
        }

        public void UpdateQuestionAndAnswer (int questionNumber, String question, String answer1, String answer2, String answer3, String answer4, int correctAnswer)
        {
            String currentScoreQuery = "";
            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    MySqlCommand myCommand = new MySqlCommand();

                    currentScoreQuery =
                        "DELETE FROM UserAnswer; " +
                        "DELETE FROM UserGames; " +
                        "DELETE FROM Users; " +
                        "DELETE FROM TestAnswers WHERE QuestionNumber = " + questionNumber + "; " +
                        "DELETE FROM TestQuestions WHERE QuestionNumber = " + questionNumber + "; " +
                        "INSERT INTO TestQuestions (QuestionNumber, Question) VALUES (" + questionNumber + ", '" + question + "'); ";

                    if (correctAnswer == 1)
                    {
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer1 + "', TRUE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer2 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer3 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer4 + "', FALSE); ";
                    }

                    else if (correctAnswer == 2)
                    {
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer1 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer2 + "', TRUE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer3 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer4 + "', FALSE); ";
                    }

                    else if (correctAnswer == 3)
                    {
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer1 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer2 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer3 + "', TRUE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer4 + "', FALSE); ";
                    }
                    else
                    {
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer1 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer2 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer3 + "', FALSE); ";
                        currentScoreQuery += "INSERT INTO TestAnswers (QuestionNumber, Answer, IsCorrect) VALUES (" + questionNumber + ", '" + answer4 + "', TRUE); ";
                    }

                    Logger.Log("Created query: " + currentScoreQuery);
                    try
                    {
                        myCommand = new MySqlCommand(currentScoreQuery, sqlConnection);

                        myCommand.ExecuteNonQuery();
                    }
                    catch(Exception)
                    {
                        Logger.Log("Error: MySql failed");
                    }
                }
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message + currentScoreQuery);
            }
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
                        currentScores += readData.GetString(0) + " " + readData.GetString(1) + "|";
                    }
                    readData.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                currentScores = "";
            }
            return currentScores;
        }

        // get average time it takes to answer a question correctly
        public float GetAverageTimeToAnswerCorrectly(int questionNumber)
        {
            float averageTime = 0;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String averageQuery = "SELECT AVG(AnswerScore) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore>0;";
                    MySqlCommand myCommand = new MySqlCommand(averageQuery, sqlConnection);

                    averageTime = (float)(decimal)myCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                averageTime = 0;
            }
            return averageTime;
        }

        // get numbers of users that answered correctly
        public int GetNumberOfUsersThatAnsweredCorrectly(int questionNumber)
        {
            int usersThatWereCorrect = 0;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String correctQuery = "SELECT count(*) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore>0;";
                    MySqlCommand myCommand = new MySqlCommand(correctQuery, sqlConnection);

                    usersThatWereCorrect = (int)(long)myCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                usersThatWereCorrect = 0;
            }
            return usersThatWereCorrect;
        }

        // get numbers of users that answered incorrectly
        public int GetNumberOfUsersThatAnsweredIncorrectly(int questionNumber)
        {
            int usersThatWereIncorrect = 0;

            try
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    String incorrectQuery = "SELECT count(*) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore=0;";
                    MySqlCommand myCommand = new MySqlCommand(incorrectQuery, sqlConnection);

                    usersThatWereIncorrect = (int)(long)myCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                usersThatWereIncorrect = 0;
            }
            return usersThatWereIncorrect;
        }

        // Get the percentage of users who answered correctly
        public float GetPercentOfUsersWhoAnsweredCorrectly(int questionNumber)
        {
            float percent;

            try
            {
                int numberWhoWereRight = this.GetNumberOfUsersThatAnsweredCorrectly(questionNumber);
                int numberWhoWereWrong = this.GetNumberOfUsersThatAnsweredIncorrectly(questionNumber);
                int totalUsers = numberWhoWereRight + numberWhoWereWrong;

                if (totalUsers != 0)
                {
                    percent = ((float)numberWhoWereRight / (float)totalUsers) * 100;
                }
                else
                {
                    percent = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                percent = 0;
            }
            return percent;
        }
    }
}
