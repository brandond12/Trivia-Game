namespace TriviaGameUser
{
    partial class Answers
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
            this.lbl_questionLable = new System.Windows.Forms.Label();
            this.lbl_gameQuestion = new System.Windows.Forms.Label();
            this.chkbx_answer1 = new System.Windows.Forms.CheckBox();
            this.chkbx_answer2 = new System.Windows.Forms.CheckBox();
            this.chkbx_answer3 = new System.Windows.Forms.CheckBox();
            this.chkbx_answer4 = new System.Windows.Forms.CheckBox();
            this.btn_Next = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_questionLable
            // 
            this.lbl_questionLable.AutoSize = true;
            this.lbl_questionLable.Location = new System.Drawing.Point(13, 13);
            this.lbl_questionLable.Name = "lbl_questionLable";
            this.lbl_questionLable.Size = new System.Drawing.Size(55, 13);
            this.lbl_questionLable.TabIndex = 0;
            this.lbl_questionLable.Text = "Question: ";
            // 
            // lbl_gameQuestion
            // 
            this.lbl_gameQuestion.AutoSize = true;
            this.lbl_gameQuestion.Location = new System.Drawing.Point(74, 13);
            this.lbl_gameQuestion.Name = "lbl_gameQuestion";
            this.lbl_gameQuestion.Size = new System.Drawing.Size(49, 13);
            this.lbl_gameQuestion.TabIndex = 1;
            this.lbl_gameQuestion.Text = "Question";
            // 
            // chkbx_answer1
            // 
            this.chkbx_answer1.AutoSize = true;
            this.chkbx_answer1.Location = new System.Drawing.Point(16, 40);
            this.chkbx_answer1.Name = "chkbx_answer1";
            this.chkbx_answer1.Size = new System.Drawing.Size(70, 17);
            this.chkbx_answer1.TabIndex = 2;
            this.chkbx_answer1.Text = "Answer 1";
            this.chkbx_answer1.UseVisualStyleBackColor = true;
            // 
            // chkbx_answer2
            // 
            this.chkbx_answer2.AutoSize = true;
            this.chkbx_answer2.Location = new System.Drawing.Point(16, 73);
            this.chkbx_answer2.Name = "chkbx_answer2";
            this.chkbx_answer2.Size = new System.Drawing.Size(70, 17);
            this.chkbx_answer2.TabIndex = 3;
            this.chkbx_answer2.Text = "Answer 2";
            this.chkbx_answer2.UseVisualStyleBackColor = true;
            // 
            // chkbx_answer3
            // 
            this.chkbx_answer3.AutoSize = true;
            this.chkbx_answer3.Location = new System.Drawing.Point(13, 107);
            this.chkbx_answer3.Name = "chkbx_answer3";
            this.chkbx_answer3.Size = new System.Drawing.Size(70, 17);
            this.chkbx_answer3.TabIndex = 4;
            this.chkbx_answer3.Text = "Answer 3";
            this.chkbx_answer3.UseVisualStyleBackColor = true;
            // 
            // chkbx_answer4
            // 
            this.chkbx_answer4.AutoSize = true;
            this.chkbx_answer4.Location = new System.Drawing.Point(13, 143);
            this.chkbx_answer4.Name = "chkbx_answer4";
            this.chkbx_answer4.Size = new System.Drawing.Size(70, 17);
            this.chkbx_answer4.TabIndex = 5;
            this.chkbx_answer4.Text = "Answer 4";
            this.chkbx_answer4.UseVisualStyleBackColor = true;
            // 
            // btn_Next
            // 
            this.btn_Next.Location = new System.Drawing.Point(304, 173);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 23);
            this.btn_Next.TabIndex = 6;
            this.btn_Next.Text = "Next";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // Answers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 208);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.chkbx_answer4);
            this.Controls.Add(this.chkbx_answer3);
            this.Controls.Add(this.chkbx_answer2);
            this.Controls.Add(this.chkbx_answer1);
            this.Controls.Add(this.lbl_gameQuestion);
            this.Controls.Add(this.lbl_questionLable);
            this.Name = "Answers";
            this.Text = "Answers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_questionLable;
        private System.Windows.Forms.Label lbl_gameQuestion;
        private System.Windows.Forms.CheckBox chkbx_answer1;
        private System.Windows.Forms.CheckBox chkbx_answer2;
        private System.Windows.Forms.CheckBox chkbx_answer3;
        private System.Windows.Forms.CheckBox chkbx_answer4;
        private System.Windows.Forms.Button btn_Next;
    }
}