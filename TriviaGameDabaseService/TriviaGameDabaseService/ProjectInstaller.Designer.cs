namespace TriviaGameDabaseService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.triviaGameServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.triviaGameServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // triviaGameServiceProcessInstaller
            // 
            this.triviaGameServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.triviaGameServiceProcessInstaller.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.triviaGameServiceInstaller});
            this.triviaGameServiceProcessInstaller.Password = null;
            this.triviaGameServiceProcessInstaller.Username = null;
            // 
            // triviaGameServiceInstaller
            // 
            this.triviaGameServiceInstaller.ServiceName = "triviaGameService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.triviaGameServiceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller triviaGameServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller triviaGameServiceInstaller;
    }
}