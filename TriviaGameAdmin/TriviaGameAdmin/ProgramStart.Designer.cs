namespace TriviaGameAdmin
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
            this.lbl_Name = new System.Windows.Forms.Label();
            this.txtbx_name = new System.Windows.Forms.TextBox();
            this.lbl_ServerName = new System.Windows.Forms.Label();
            this.txtbx_server = new System.Windows.Forms.TextBox();
            this.btn_submit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Name
            // 
            this.lbl_Name.AutoSize = true;
            this.lbl_Name.Location = new System.Drawing.Point(12, 20);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(67, 13);
            this.lbl_Name.TabIndex = 0;
            this.lbl_Name.Text = "Admin Name";
            // 
            // txtbx_name
            // 
            this.txtbx_name.Location = new System.Drawing.Point(87, 17);
            this.txtbx_name.Name = "txtbx_name";
            this.txtbx_name.Size = new System.Drawing.Size(242, 20);
            this.txtbx_name.TabIndex = 1;
            // 
            // lbl_ServerName
            // 
            this.lbl_ServerName.AutoSize = true;
            this.lbl_ServerName.Location = new System.Drawing.Point(17, 60);
            this.lbl_ServerName.Name = "lbl_ServerName";
            this.lbl_ServerName.Size = new System.Drawing.Size(69, 13);
            this.lbl_ServerName.TabIndex = 2;
            this.lbl_ServerName.Text = "Server Name";
            // 
            // txtbx_server
            // 
            this.txtbx_server.Location = new System.Drawing.Point(87, 57);
            this.txtbx_server.Name = "txtbx_server";
            this.txtbx_server.Size = new System.Drawing.Size(237, 20);
            this.txtbx_server.TabIndex = 3;
            this.txtbx_server.Text = ".";
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(254, 98);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(75, 23);
            this.btn_submit.TabIndex = 4;
            this.btn_submit.Text = "Submit";
            this.btn_submit.UseVisualStyleBackColor = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // ProgramStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 139);
            this.Controls.Add(this.btn_submit);
            this.Controls.Add(this.txtbx_server);
            this.Controls.Add(this.lbl_ServerName);
            this.Controls.Add(this.txtbx_name);
            this.Controls.Add(this.lbl_Name);
            this.Name = "ProgramStart";
            this.Text = "ProgramStart";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txtbx_name;
        private System.Windows.Forms.Label lbl_ServerName;
        private System.Windows.Forms.TextBox txtbx_server;
        private System.Windows.Forms.Button btn_submit;
    }
}