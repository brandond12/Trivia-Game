namespace TriviaGameAdmin
{
    partial class MainWindow
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
            this.btn_EditQuestion = new System.Windows.Forms.Button();
            this.btn_CurrentStatus = new System.Windows.Forms.Button();
            this.btn_Leaderboard = new System.Windows.Forms.Button();
            this.btn_ExportExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_EditQuestion
            // 
            this.btn_EditQuestion.Location = new System.Drawing.Point(13, 13);
            this.btn_EditQuestion.Name = "btn_EditQuestion";
            this.btn_EditQuestion.Size = new System.Drawing.Size(80, 23);
            this.btn_EditQuestion.TabIndex = 0;
            this.btn_EditQuestion.Text = "Edit Question";
            this.btn_EditQuestion.UseVisualStyleBackColor = true;
            this.btn_EditQuestion.Click += new System.EventHandler(this.btn_EditQuestion_Click);
            // 
            // btn_CurrentStatus
            // 
            this.btn_CurrentStatus.Location = new System.Drawing.Point(100, 13);
            this.btn_CurrentStatus.Name = "btn_CurrentStatus";
            this.btn_CurrentStatus.Size = new System.Drawing.Size(82, 23);
            this.btn_CurrentStatus.TabIndex = 1;
            this.btn_CurrentStatus.Text = "Current Status";
            this.btn_CurrentStatus.UseVisualStyleBackColor = true;
            this.btn_CurrentStatus.Click += new System.EventHandler(this.btn_CurrentStatus_Click);
            // 
            // btn_Leaderboard
            // 
            this.btn_Leaderboard.Location = new System.Drawing.Point(188, 13);
            this.btn_Leaderboard.Name = "btn_Leaderboard";
            this.btn_Leaderboard.Size = new System.Drawing.Size(78, 23);
            this.btn_Leaderboard.TabIndex = 2;
            this.btn_Leaderboard.Text = "Leaderboard";
            this.btn_Leaderboard.UseVisualStyleBackColor = true;
            this.btn_Leaderboard.Click += new System.EventHandler(this.btn_Leaderboard_Click);
            // 
            // btn_ExportExcel
            // 
            this.btn_ExportExcel.Location = new System.Drawing.Point(273, 13);
            this.btn_ExportExcel.Name = "btn_ExportExcel";
            this.btn_ExportExcel.Size = new System.Drawing.Size(79, 23);
            this.btn_ExportExcel.TabIndex = 3;
            this.btn_ExportExcel.Text = "Export Excel";
            this.btn_ExportExcel.UseVisualStyleBackColor = true;
            this.btn_ExportExcel.Click += new System.EventHandler(this.btn_ExportExcel_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 58);
            this.Controls.Add(this.btn_ExportExcel);
            this.Controls.Add(this.btn_Leaderboard);
            this.Controls.Add(this.btn_CurrentStatus);
            this.Controls.Add(this.btn_EditQuestion);
            this.Name = "MainWindow";
            this.Text = "Admin";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_EditQuestion;
        private System.Windows.Forms.Button btn_CurrentStatus;
        private System.Windows.Forms.Button btn_Leaderboard;
        private System.Windows.Forms.Button btn_ExportExcel;

    }
}

