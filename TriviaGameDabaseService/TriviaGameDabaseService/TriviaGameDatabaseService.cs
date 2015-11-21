using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TriviaGameDabaseService
{
    public partial class TriviaGameDatabaseService : ServiceBase
        {
        Thread t;
        bool Done;
        public TriviaGameDatabaseService()
        {
            InitializeComponent();
            Done = false;
            CanPauseAndContinue = true;
            t = new Thread(new ThreadStart(listener));
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("Started Service");
            t.Start();
        }

        protected override void OnStop()
        {
            Done = true; 
            t.Join();
            Logger.Log("stopped service");
        }

        protected override void OnPause()
        {
            t.Suspend();
            Logger.Log("paused service");
        }

        protected override void OnContinue()
        {
            t.Resume();
            Logger.Log("continue service");
        }

        private void listener()
        {
            while(!Done)
            {
                Logger.Log("service running");
                Thread.Sleep(1000);
            }
        }
    }
}
