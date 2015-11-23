namespace TriviaGameUser
{
    partial class ProgramStart
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
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbx_userName = new System.Windows.Forms.TextBox();
            this.txtbx_serverName = new System.Windows.Forms.TextBox();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(12, 26);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(60, 13);
            this.lbl_UserName.TabIndex = 0;
            this.lbl_UserName.Text = "Your Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server Computer Name";
            // 
            // txtbx_userName
            // 
            this.txtbx_userName.Location = new System.Drawing.Point(110, 26);
            this.txtbx_userName.Name = "txtbx_userName";
            this.txtbx_userName.Size = new System.Drawing.Size(191, 20);
            this.txtbx_userName.TabIndex = 2;
            this.txtbx_userName.Text = "TestPerson";
            // 
            // txtbx_serverName
            // 
            this.txtbx_serverName.Location = new System.Drawing.Point(157, 70);
            this.txtbx_serverName.Name = "txtbx_serverName";
            this.txtbx_serverName.Size = new System.Drawing.Size(144, 20);
            this.txtbx_serverName.TabIndex = 3;
            this.txtbx_serverName.Text = ".";
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(225, 128);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(75, 23);
            this.btn_Submit.TabIndex = 4;
            this.btn_Submit.Text = "Ok";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // ProgramStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 185);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.txtbx_serverName);
            this.Controls.Add(this.txtbx_userName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_UserName);
            this.Name = "ProgramStart";
            this.Text = "ProgramStart";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbx_userName;
        private System.Windows.Forms.TextBox txtbx_serverName;
        private System.Windows.Forms.Button btn_Submit;
    }
}