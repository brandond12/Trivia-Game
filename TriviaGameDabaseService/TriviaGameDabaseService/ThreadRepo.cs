/*
* FILE   : ThreadRepo.cs
* PROJECT  : PROG2120 - Windows and Mobile Programing - PROG 2110 Relation Database - Trivia Game 
* PROGRAMMER : Brandon Davies - Lauren Machan
* FIRST VERSION : 2015-11-27
* DESCRIPTION : This class is used as a repository for the threads
 *              the thread and the name of the pipe the thread is connected to are saved
 *              the pipe name is used as the ID for the thread
*/

using System;
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

        /*
        *METHOD		    :	ThreadRepo
        *
        *DESCRIPTION	:	Constructor for thread repo
        *
        *PARAMETERS		:	void
        *  
        *RETURNS		:	
        *
        */
        public ThreadRepo()
        {
            Threads = new List<Thread>();
            clientPipes = new List<string>();
        }

        /*
        *METHOD		    :	AddClient
        *
        *DESCRIPTION	:	Adds a client to the repo. holds the thread and pipeName fir ID
        *
        *PARAMETERS		:	Thread thread           Thread to add
         *                  string pipeName         name of pipe to add
        *  
        *RETURNS		:	
        *
        */
        public void AddClient(Thread thread, string pipeName)
        {
            Threads.Add(thread);
            clientPipes.Add(pipeName);
        }

        /*
        *METHOD		    :	DeleteClient
        *
        *DESCRIPTION	:	Deletes a user from repo based on pipe name
        *
        *PARAMETERS		:	string pipeName         name of pipe of user to delete
        *  
        *RETURNS		:	    void
        *
        */
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
