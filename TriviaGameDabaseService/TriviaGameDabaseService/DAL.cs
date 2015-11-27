using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace TriviaGameDabaseService
{
    class DAL
    {
        private MySqlConnection sqlConnection;

        /*
        *METHOD		    :	DAL
        *
        *DESCRIPTION	:	Constructor that gets the database connection string information from a method
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        public DAL()
        {
            String filePath = @"C:\databaseConnectionInfo.txt";
            //get a connection string from a method and create the connection
            String connectionString = this.GetConnectionString(filePath);
            sqlConnection = new MySqlConnection(connectionString);
        }

        /*
        *METHOD		    :	GetConnectionString
        *
        *DESCRIPTION	:	Method to open a file, read the variables for a connection 
        *                   string from the file, and return a connection string to the 
        *                   calling method
        *
        *PARAMETERS		:	String filePath     Path to the file that contains the database connection information
        *  
        *RETURNS		:	String      The database connection string
        *
        */
        private String GetConnectionString(String filePath)
        {
            String fileLine = "";
            String server = "";
            String database = "";
            String uID = "";
            String password = "";
            String[] connectionPairs;
            String connectionString = "";

            try
            {   //open the text file using a stream reader
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        //read a line from the file and split it
                        fileLine = sr.ReadLine();
                        connectionPairs = fileLine.Split('=');
                        //set the connection string variables depending on the parameter name
                        if (connectionPairs[0] == "server")
                        {
                            server = connectionPairs[1];
                        }
                        else if (connectionPairs[0] == "database")
                        {
                            database = connectionPairs[1];
                        }
                        else if (connectionPairs[0] == "uID")
                        {
                            uID = connectionPairs[1];
                        }
                        else if (connectionPairs[0] == "password")
                        {
                            password = connectionPairs[1];
                        }
                    }
                    //create a connection string from the file's values
                    connectionString = "server=" + server + ";" +
                                       "database=" + database + ";" +
                                       "uid=" + uID + ";" +
                                       "pwd=" + password + ";";
                }
            }
            catch (Exception)
            {
                Logger.Log("The connection string to the database failed.");
                connectionString = "";
            }
            return connectionString;
        }

        /*
        *METHOD		    :	OpenConnection
        *
        *DESCRIPTION	:	Method to open the database connection
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        public void OpenConnection()
        {
            try
            {
                //open the database connection
                sqlConnection.Open();
                Logger.Log("Connection to database succeeded.");
            }
            catch (Exception)
            {
                Logger.Log("The database connection couldn't be opened.");
            }
        }

        /*
        *METHOD		    :	CloseConnection
        *
        *DESCRIPTION	:	Method to close the database connection
        *
        *PARAMETERS		:	
        *  
        *RETURNS		:	void
        *
        */
        public void CloseConnection()
        {
            try
            {
                //close the database connection
                sqlConnection.Close();
                Logger.Log("Closing of the database connection succeeded.");
            }
            catch (Exception)
            {
                Logger.Log("The database connection couldn't be closed.");
            }
        }

        /*
        *METHOD		    :	GetQuestion
        *
        *DESCRIPTION	:	Method retrieves a specific question from the database
        *
        *PARAMETERS		:	int questionNumber     The question to retrieve
        *  
        *RETURNS		:	String      The question text
        *
        */
        public String GetQuestion(int questionNumber)
        {
            String question = "";
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get a question
                    String questionQuery = "SELECT Question FROM TestQuestions WHERE QuestionNumber=" + questionNumber + ";";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(questionQuery, sqlConnection);
                    question = Convert.ToString(myCommand.ExecuteScalar());
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return question;
        }

        /*
        *METHOD		    :	GetQuestionAndAnswers
        *
        *DESCRIPTION	:	Method retrieves a specific question and its answers from the database
        *
        *PARAMETERS		:	int questionNumber     The question and answers to retrieve
        *  
        *RETURNS		:	String      The question and answers text
        *
        */
        public String GetQuestionAndAnswers(int questionNumber)
        {
            String response = "";
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //get the question, then get the answers for that specific question
                    response += this.GetQuestion(questionNumber) + "|";
                    response += this.GetAnswers(questionNumber);
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return response;
        }

        /*
        *METHOD		    :	GetAnswers
        *
        *DESCRIPTION	:	Method retrieves a specific question's answers from the database
        *
        *PARAMETERS		:	int questionNumber     The question's answers to retrieve
        *  
        *RETURNS		:	String      The answers text
        *
        */
        public String GetAnswers(int questionNumber)
        {
            String answers = "";
            String currentAnswer = "";
            String correctAnswer = "";
            int correctAnswerLocation = 0;
            int counter = 1;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get the answers, and create the command
                    String answersQuery = "SELECT Answer From TestAnswers WHERE QuestionNumber=" + questionNumber + ";";
                    MySqlCommand myCommand = new MySqlCommand(answersQuery, sqlConnection);
                    //get the text for the correct answer
                    correctAnswer = this.GetCorrectAnswer(questionNumber);

                    //create a data reader to read the answers
                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        //get an answer from the database and add it to the answer string
                        currentAnswer = (String)(readData["Answer"]);
                        answers += currentAnswer + "|";
                        //check if the recently retrieved answer is the correct answer
                        if (currentAnswer == correctAnswer)
                        {
                            //set the answer's location
                            correctAnswerLocation = counter;
                        }
                        counter++;
                    }
                    readData.Close();
                    //add the answer's location to the end of the string
                    answers += correctAnswerLocation.ToString();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return answers;
        }

        /*
        *METHOD		    :	StoreUserName
        *
        *DESCRIPTION	:	Method used to store usernames in the database
        *
        *PARAMETERS		:	String name     The username to store
        *  
        *RETURNS		:	void
        *
        */
        public void StoreUserName(String name)
        {
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will add the username to the database
                    String nameQuery = "INSERT INTO Users (Name, IsActive) VALUES ('" + name + "', TRUE);";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(nameQuery, sqlConnection);
                    myCommand.ExecuteNonQuery();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	InitializeUserInLeaderboard
        *
        *DESCRIPTION	:	Method used to store usernames in the database
        *
        *PARAMETERS		:	String userName     The user
        *                   int gameNumber      The game number the user is on
        *  
        *RETURNS		:	void
        *
        */
        public void InitializeUserInLeaderboard(String userName, int gameNumber)
        {
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will add the user to the leaderboard
                    String leaderboardQuery = "INSERT INTO UserGames (GameNumber, Name, GameScore) VALUES (" + gameNumber + ", '" + userName + "', 0);";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(leaderboardQuery, sqlConnection);
                    myCommand.ExecuteNonQuery();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	StoreUserAnswer
        *
        *DESCRIPTION	:	Method used to store a user's answer in the database
        *
        *PARAMETERS		:	String userName    The user
        *                   int gameNumber     The game number the user is on
        *                   int gameQuestion   The question that the user is on
        *                   String userAnswer  The text version of the user's answer
        *                   int answerScore    The user's score for this question
        *  
        *RETURNS		:	void
        *
        */
        public void StoreUserAnswer(String userName, int gameNumber, int gameQuestion, String userAnswer, int answerScore)
        {
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will add the user's answer into the database
                    String answerQuery = "INSERT INTO UserAnswer (GameNumber, Name, GameQuestion, UserAnswer, AnswerScore)"
                    + "VALUES (" + gameNumber + ", '" + userName + "', " + gameQuestion + ", '" + userAnswer + "', " + answerScore + ");";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(answerQuery, sqlConnection);
                    myCommand.ExecuteNonQuery();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	AlterLeaderboard
        *
        *DESCRIPTION	:	Method used to alter a user's score in the leaderboard
        *
        *PARAMETERS		:	int gameNumber     The game number the user is on
        *                   String name        The user
        *                   int gameScore      The user's total game score
        *  
        *RETURNS		:	void
        *
        */
        public void AlterLeaderboard(int gameNumber, String name, int gameScore)
        {
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will update a user's score
                    String updateQuery = "UPDATE UserGames SET GameScore=" + gameScore + " WHERE Name='" + name + "' AND GameNumber=" + gameNumber + ";";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(updateQuery, sqlConnection);
                    myCommand.ExecuteNonQuery();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	Leaderboard
        *
        *DESCRIPTION	:	Method retrieves the leaderboard from the database
        *
        *PARAMETERS		:	int gameNumber     The game number the user is on
        *  
        *RETURNS		:	String      The leaderboard text
        *
        */
        public String Leaderboard(int gameNumber)
        {
            String leaderboard = "";
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will retrieve the leaderboard, and create the command
                    String leaderboardQuery = "SELECT Name, GameScore FROM UserGames WHERE GameNumber=" + gameNumber + " ORDER BY GameScore DESC;";
                    MySqlCommand myCommand = new MySqlCommand(leaderboardQuery, sqlConnection);

                    //create a data reader to read users and their scores
                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        //add a user and their score to the leaderboard string
                        leaderboard += readData.GetString(0) + " " + readData.GetString(1) + "|";
                    }
                    readData.Close();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                leaderboard = "";
            }
            return leaderboard;
        }


        /*
        *METHOD		    :	SetUserToInactive
        *
        *DESCRIPTION	:	Method sets a specific user to inactive
        *
        *PARAMETERS		:	String name     The user to set to inactive
        *  
        *RETURNS		:	void
        *
        */
        public void SetUserToInactive(String name)
        {
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will set a user to inactive
                    String inactiveQuery = "UPDATE Users SET IsActive=FALSE WHERE Name='" + name + "';";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(inactiveQuery, sqlConnection);
                    myCommand.ExecuteNonQuery();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
        }

        /*
        *METHOD		    :	IsNameInDatabase
        *
        *DESCRIPTION	:	Method checks if a username has been used or not
        *
        *PARAMETERS		:	String name     The username to check
        *  
        *RETURNS		:	bool    Is the name taken or not
        *
        */
        public bool IsNameInDatabase(String name)
        {
            bool isNameTaken = false;
            int nameCount = 0;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will retrieve a count, and create the command
                    String nameQuery = "SELECT count(*) FROM Users WHERE Name='" + name + "';";
                    MySqlCommand myCommand = new MySqlCommand(nameQuery, sqlConnection);

                    //get a count on the users with the specified name
                    nameCount = Convert.ToInt32(myCommand.ExecuteScalar());
                    //if the count is 0, then the username is free
                    if (nameCount == 0)
                    {
                        isNameTaken = false;
                    }
                    //otherwise the name is taken
                    else
                    {
                        isNameTaken = true;
                    }
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                isNameTaken = true;
            }
            return isNameTaken;
        }

        /*
        *METHOD		    :	GetCorrectAnswer
        *
        *DESCRIPTION	:	Method that gets the correct answer for a specific question
        *
        *PARAMETERS		:	int questionNumber      The question to get the correct answer for
        *  
        *RETURNS		:	String    The correct answer text
        *
        */
        public String GetCorrectAnswer(int questionNumber)
        {
            String correctAnswer = "";
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get the correct answer for a question
                    String answerQuery = "SELECT Answer FROM TestAnswers WHERE QuestionNumber=" + questionNumber + " AND IsCorrect=TRUE;";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(answerQuery, sqlConnection);
                    correctAnswer = Convert.ToString(myCommand.ExecuteScalar());
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
            }
            return correctAnswer;
        }

        public void UpdateQuestionAndAnswer(int questionNumber, String question, String answer1, String answer2, String answer3, String answer4, int correctAnswer)
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

        /*
        *METHOD		    :	GetCurrentStatus
        *
        *DESCRIPTION	:	Method that gets the current status for active users
        *
        *PARAMETERS		:	int gameNumber      The game number the user is on
        *  
        *RETURNS		:	String    The current status text
        *
        */
        public String GetCurrentStatus(int gameNumber)
        {
            String currentScores = "";
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will retrieve the current status, and create the command
                    String currentScoreQuery = "SELECT ug.Name, SUM(AnswerScore) as 'Score' FROM UserAnswer ua INNER JOIN UserGames ug" +
                    " ON ua.Name = ug.Name INNER JOIN Users u ON ug.Name = u.Name WHERE ug.GameNumber=" + gameNumber + " AND IsActive=TRUE" +
                    " GROUP BY Name ORDER BY SUM(AnswerScore) DESC;";
                    MySqlCommand myCommand = new MySqlCommand(currentScoreQuery, sqlConnection);

                    //create a data reader to read users and their scores
                    MySqlDataReader readData = myCommand.ExecuteReader();
                    while (readData.Read())
                    {
                        //add a user and their score to the current score string
                        currentScores += readData.GetString(0) + " " + readData.GetString(1) + "|";
                    }
                    readData.Close();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                currentScores = "";
            }
            return currentScores;
        }

        /*
        *METHOD		    :	GetAverageTimeToAnswerCorrectly
        *
        *DESCRIPTION	:	Method that gets the average time to answer a specific question correctly
        *
        *PARAMETERS		:	int questionNumber      The question to get the correct answer for
        *  
        *RETURNS		:	float    The average time to answer a question correctly
        *
        */
        public float GetAverageTimeToAnswerCorrectly(int questionNumber)
        {
            float averageTime = 0;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get the average correct answer time
                    String averageQuery = "SELECT AVG(AnswerScore) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore>0;";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(averageQuery, sqlConnection);
                    averageTime = (float)(decimal)myCommand.ExecuteScalar();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                averageTime = 0;
            }
            return averageTime;
        }

        /*
        *METHOD		    :	GetNumberOfUsersThatAnsweredCorrectly
        *
        *DESCRIPTION	:	Method that gets the number of users who answered a specific question correctly
        *
        *PARAMETERS		:	int questionNumber      How many users answered this question number correctly
        *  
        *RETURNS		:	int    The number of users who answered the question correctly
        *
        */
        public int GetNumberOfUsersThatAnsweredCorrectly(int questionNumber)
        {
            int usersThatWereCorrect = 0;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get the count of users who answered a question correctly
                    String correctQuery = "SELECT count(*) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore>0;";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(correctQuery, sqlConnection);
                    usersThatWereCorrect = (int)(long)myCommand.ExecuteScalar();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                usersThatWereCorrect = 0;
            }
            return usersThatWereCorrect;
        }

        /*
        *METHOD		    :	GetNumberOfUsersThatAnsweredIncorrectly
        *
        *DESCRIPTION	:	Method that gets the number of users who answered a specific question incorrectly
        *
        *PARAMETERS		:	int questionNumber      How many users answered this question number incorrectly
        *  
        *RETURNS		:	int    The number of users who answered the question incorrectly
        *
        */
        public int GetNumberOfUsersThatAnsweredIncorrectly(int questionNumber)
        {
            int usersThatWereIncorrect = 0;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //set a query that will get the count of users who answered a question incorrectly
                    String incorrectQuery = "SELECT count(*) FROM UserAnswer WHERE GameQuestion=" + questionNumber + " AND AnswerScore=0;";
                    //create the command, and execute the command
                    MySqlCommand myCommand = new MySqlCommand(incorrectQuery, sqlConnection);
                    usersThatWereIncorrect = (int)(long)myCommand.ExecuteScalar();
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex.Message);
                usersThatWereIncorrect = 0;
            }
            return usersThatWereIncorrect;
        }

        /*
        *METHOD		    :	GetPercentOfUsersWhoAnsweredCorrectly
        *
        *DESCRIPTION	:	Method that gets the percentage of users who answered a specific question correctly
        *
        *PARAMETERS		:	int questionNumber      How many users answered this question number correctly
        *  
        *RETURNS		:	float    The percentage of users who answered the question correctly
        *
        */
        public float GetPercentOfUsersWhoAnsweredCorrectly(int questionNumber)
        {
            float percent = 0;
            try
            {
                //check that the connection is open
                if (sqlConnection.State == ConnectionState.Open)
                {
                    //get the number of users who were right and wrong, and add them for a total user count
                    int numberWhoWereRight = this.GetNumberOfUsersThatAnsweredCorrectly(questionNumber);
                    int numberWhoWereWrong = this.GetNumberOfUsersThatAnsweredIncorrectly(questionNumber);
                    int totalUsers = numberWhoWereRight + numberWhoWereWrong;

                    //check if the total user amount isn't 0
                    if (totalUsers != 0)
                    {
                        //calculate the percent of users who were right
                        percent = ((float)numberWhoWereRight / (float)totalUsers) * 100;
                    }
                    //if total users is 0, then the percent is 0
                    else
                    {
                        percent = 0;
                    }
                }
                //write to the log if the connection is closed
                else
                {
                    Logger.Log("Error: Connection not open");
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
