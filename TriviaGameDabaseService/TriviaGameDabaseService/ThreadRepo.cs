﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Pipes;

namespace TriviaGameDabaseService
{
    class ThreadRepo
    {
        private List<Thread> Threads;
        private List<string> clientPipes;

        public ThreadRepo()
        {
            Threads = new List<Thread>();
            clientPipes = new List<string>();
        }

        public void AddClient(Thread thread, string pipeName)
        {
            Threads.Add(thread);
            clientPipes.Add(pipeName);
        }

        public void DeleteClient(string pipeName)
        {
            int count = 0;
            foreach(string name in clientPipes)
            {
                //if pipe names match, they are the a match
                //remove from the lists
                if(name == pipeName)
                {
                    Threads.RemoveAt(count);
                    clientPipes.RemoveAt(count);
                    break;
                }
                count++;
            }
        }
    }
}
