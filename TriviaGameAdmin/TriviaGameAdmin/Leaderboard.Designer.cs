namespace TriviaGameAdmin
{
    partial class Leaderboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtbx_Leaderboard = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtbx_Leaderboard
            // 
            this.txtbx_Leaderboard.AllowDrop = true;
            this.txtbx_Leaderboard.Location = new System.Drawing.Point(13, 13);
            this.txtbx_Leaderboard.Multiline = true;
            this.txtbx_Leaderboard.Name = "txtbx_Leaderboard";
            this.txtbx_Leaderboard.ReadOnly = true;
            this.txtbx_Leaderboard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbx_Leaderboard.Size = new System.Drawing.Size(543, 254);
            this.txtbx_Leaderboard.TabIndex = 0;
            // 
            // Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 279);
            this.Controls.Add(this.txtbx_Leaderboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Leaderboard";
            this.Text = "Leaderboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbx_Leaderboard;
    }
}