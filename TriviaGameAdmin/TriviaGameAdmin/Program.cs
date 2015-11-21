using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TriviaGameAdmin
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
            ProgramStart gameStart = new ProgramStart();
            Application.Run(gameStart);
            Application.Run(new MainWindow(gameStart.getUserName(), gameStart.getPipeName()));
        }
    }
}
