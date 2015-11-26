using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TriviaGameUser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                ProgramStart gameStart = new ProgramStart();
                Application.Run(gameStart);
                Application.Run(new QuestionWindow(gameStart.getUserName(), gameStart.getServerName(), gameStart.getPipeName()));
            }
            catch(Exception ex)
            {
                //game was closed before starting
            }
        }
    }
}
