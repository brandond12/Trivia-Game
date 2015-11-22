namespace TriviaGameUser
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
            this.lbl_Leaderboard = new System.Windows.Forms.Label();
            this.txtbx_Leaderboard = new System.Windows.Forms.TextBox();
            this.btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Leaderboard
            // 
            this.lbl_Leaderboard.AutoSize = true;
            this.lbl_Leaderboard.Location = new System.Drawing.Point(13, 13);
            this.lbl_Leaderboard.Name = "lbl_Leaderboard";
            this.lbl_Leaderboard.Size = new System.Drawing.Size(67, 13);
            this.lbl_Leaderboard.TabIndex = 0;
            this.lbl_Leaderboard.Text = "Leaderboard";
            // 
            // txtbx_Leaderboard
            // 
            this.txtbx_Leaderboard.Location = new System.Drawing.Point(13, 30);
            this.txtbx_Leaderboard.Multiline = true;
            this.txtbx_Leaderboard.Name = "txtbx_Leaderboard";
            this.txtbx_Leaderboard.ReadOnly = true;
            this.txtbx_Leaderboard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbx_Leaderboard.Size = new System.Drawing.Size(377, 196);
            this.txtbx_Leaderboard.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(315, 232);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 267);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.txtbx_Leaderboard);
            this.Controls.Add(this.lbl_Leaderboard);
            this.Name = "Leaderboard";
            this.Text = "EndGame";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Leaderboard;
        private System.Windows.Forms.TextBox txtbx_Leaderboard;
        private System.Windows.Forms.Button btn_Close;
    }
}