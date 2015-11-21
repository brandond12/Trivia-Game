using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace TriviaGameDabaseService
{
    //syslog is more generic and is more powerful, it can allow you to log from all platforms , info is caught from across the net
    public static class Logger
    {
        public static void Log(string message)
        {
            EventLog serviceEventLog = new EventLog();
            if (!EventLog.SourceExists("TriviaGame-Service"))
            {
                EventLog.CreateEventSource("TriviaGame-Service", "TriviaGameEvents");
            }
            serviceEventLog.Source = "TriviaGame-Service";
            serviceEventLog.Log = "TriviaGameEvents";
            serviceEventLog.WriteEntry(message);
        }
    }
}
