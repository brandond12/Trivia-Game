namespace TriviaGameAdmin
{
    partial class CurrentStatus
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
            this.txtbx_CurrectStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtbx_CurrectStatus
            // 
            this.txtbx_CurrectStatus.Location = new System.Drawing.Point(13, 13);
            this.txtbx_CurrectStatus.Multiline = true;
            this.txtbx_CurrectStatus.Name = "txtbx_CurrectStatus";
            this.txtbx_CurrectStatus.ReadOnly = true;
            this.txtbx_CurrectStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbx_CurrectStatus.Size = new System.Drawing.Size(542, 255);
            this.txtbx_CurrectStatus.TabIndex = 0;
            // 
            // CurrentStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 280);
            this.Controls.Add(this.txtbx_CurrectStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CurrentStatus";
            this.Text = "CurrentStatus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurrentStatus_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbx_CurrectStatus;
    }
}