/*
* FILE   : Logger.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This static class creates a log method to write messages to a log from the service
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace TriviaGameDabaseService
{
    public static class Logger
    {
        /*
        *METHOD		    :	Log
        *
        *DESCRIPTION	:	Used to write a message to the log
        *
        *PARAMETERS		:	string message          message to write to the file
        *  
        *RETURNS		:	void
        *
        */
        public static void Log(string message)
        {
            //create a new event log
            EventLog serviceEventLog = new EventLog();
            //if log has not been created, make it
            if (!EventLog.SourceExists("TriviaGame-Service"))
            {
                EventLog.CreateEventSource("TriviaGame-Service", "TriviaGameEvents");
            }
            //write log source, log and message
            serviceEventLog.Source = "TriviaGame-Service";
            serviceEventLog.Log = "TriviaGameEvents";
            serviceEventLog.WriteEntry(message);
        }
    }
}
