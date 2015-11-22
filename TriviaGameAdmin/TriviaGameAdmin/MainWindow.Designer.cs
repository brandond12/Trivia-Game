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
            this.lbl_sqlCommand = new System.Windows.Forms.Label();
            this.txtbx_UserInput = new System.Windows.Forms.TextBox();
            this.lbl_sqlResponce = new System.Windows.Forms.Label();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.txtbx_ServerOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_sqlCommand
            // 
            this.lbl_sqlCommand.AutoSize = true;
            this.lbl_sqlCommand.Location = new System.Drawing.Point(13, 13);
            this.lbl_sqlCommand.Name = "lbl_sqlCommand";
            this.lbl_sqlCommand.Size = new System.Drawing.Size(78, 13);
            this.lbl_sqlCommand.TabIndex = 0;
            this.lbl_sqlCommand.Text = "SQL Command";
            // 
            // txtbx_UserInput
            // 
            this.txtbx_UserInput.Location = new System.Drawing.Point(13, 30);
            this.txtbx_UserInput.Multiline = true;
            this.txtbx_UserInput.Name = "txtbx_UserInput";
            this.txtbx_UserInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbx_UserInput.Size = new System.Drawing.Size(597, 69);
            this.txtbx_UserInput.TabIndex = 1;
            // 
            // lbl_sqlResponce
            // 
            this.lbl_sqlResponce.AutoSize = true;
            this.lbl_sqlResponce.Location = new System.Drawing.Point(13, 134);
            this.lbl_sqlResponce.Name = "lbl_sqlResponce";
            this.lbl_sqlResponce.Size = new System.Drawing.Size(105, 13);
            this.lbl_sqlResponce.TabIndex = 2;
            this.lbl_sqlResponce.Text = "Database Responce";
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(535, 106);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(75, 23);
            this.btn_Submit.TabIndex = 3;
            this.btn_Submit.Text = "Submit";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // txtbx_ServerOutput
            // 
            this.txtbx_ServerOutput.Location = new System.Drawing.Point(16, 151);
            this.txtbx_ServerOutput.Multiline = true;
            this.txtbx_ServerOutput.Name = "txtbx_ServerOutput";
            this.txtbx_ServerOutput.ReadOnly = true;
            this.txtbx_ServerOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbx_ServerOutput.Size = new System.Drawing.Size(594, 183);
            this.txtbx_ServerOutput.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 346);
            this.Controls.Add(this.txtbx_ServerOutput);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.lbl_sqlResponce);
            this.Controls.Add(this.txtbx_UserInput);
            this.Controls.Add(this.lbl_sqlCommand);
            this.Name = "MainWindow";
            this.Text = "Admin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_sqlCommand;
        private System.Windows.Forms.TextBox txtbx_UserInput;
        private System.Windows.Forms.Label lbl_sqlResponce;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.TextBox txtbx_ServerOutput;
    }
}

