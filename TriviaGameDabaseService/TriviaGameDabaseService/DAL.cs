using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

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

        // this method is a modified version of the code from https://support.microsoft.com/en-us/kb/302084
        // (still have to change the comments!!)
        public void ExcelAutomation()
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Question Number";
                oSheet.Cells[1, 2] = "Question Text";
                oSheet.Cells[1, 3] = "Average Correct Answer Time (s)";
                oSheet.Cells[1, 4] = "% Who Answered Correctly";

                //Format A1:K1 as bold, vertical alignment = center, and wrapping = true.
                oSheet.get_Range("A1", "D1").Font.Bold = true;
                oSheet.get_Range("A1", "D1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", "D1").WrapText = true;


                // Made this array of type object to store both strings and ints
                Object[,] questionArray = new Object[11, 2];

                for (int counter = 0; counter < 10; counter++)
                {
                    String questionText = this.GetQuestion(counter + 1);
                    questionArray[counter, 0] = counter + 1;
                    questionArray[counter, 1] = questionText;
                }

                //Fill A2:B6 with an array of values (Question # and Question Text).
                oSheet.get_Range("A2", "B11").Value2 = questionArray;
                oRng = oSheet.get_Range("B2", "B11");
                oRng.EntireColumn.AutoFit();

                // Get the average time to answer correctly
                float[,] averageTimes = new float[10, 10];
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentAverageTime = this.GetAverageTimeToAnswerCorrectly(counter + 1);
                    averageTimes[counter, 0] = currentAverageTime;
                }

                // Fill J2:J11 with an array of average times, and autofit the columns
                oSheet.get_Range("C2", "C11").Value2 = averageTimes;
                oRng = oSheet.get_Range("C2", "C11");
                oRng.EntireColumn.AutoFit();

                // Get the percentage of users who answered the question correctly
                float[,] percentCorrect = new float[10, 10];
                for (int counter = 0; counter < 10; counter++)
                {
                    float currentQuestionPercent = this.GetPercentOfUsersWhoAnsweredCorrectly(counter + 1);
                    percentCorrect[counter, 0] = currentQuestionPercent;
                }

                // Fill K2:K11 with an array of percents, and autofit the column
                oSheet.get_Range("D2", "D11").Value2 = percentCorrect;
                oRng = oSheet.get_Range("D2", "D11");
                oRng.EntireColumn.AutoFit();

                //Manipulate a variable number of columns for Quarterly Sales Data.
                DisplayHistogram(oSheet);

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        // In addition, a histogram should be created to show the average length of time needed to answer each question correctly
        // this method is a modified version of the code from https://support.microsoft.com/en-us/kb/302084
        public void DisplayHistogram(Excel._Worksheet oWS)
        {
            Excel._Workbook oWB;
            Excel.Range oRange;
            Excel._Chart oChart;
            Excel.Series oSeries;

            //Add a Chart for the selected data.
            oWB = (Excel._Workbook)oWS.Parent;
            oChart = (Excel._Chart)oWB.Charts.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //Use the ChartWizard to create a new chart from the selected data.
            oRange = oWS.get_Range("B1", "C11");
            oChart.SetSourceData(oRange, Missing.Value);
            oChart.ChartWizard(oRange, Excel.XlChartType.xlBarClustered, Missing.Value,
                Excel.XlRowCol.xlColumns, Missing.Value, Missing.Value, Missing.Value,
                "Average Length of Time to Answer Questions Correctly", "Questions", "Time (seconds)", Missing.Value);
            oSeries = (Excel.Series)oChart.SeriesCollection(1);
            oSeries.XValues = oWS.get_Range("B2", "B11");
        }

    }
}
