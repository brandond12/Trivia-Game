namespace TriviaGameUser
{
    partial class QuestionWindow
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
            this.lbl_Question = new System.Windows.Forms.Label();
            this.GrpBx_AnswerSelection = new System.Windows.Forms.GroupBox();
            this.rad_Answer4 = new System.Windows.Forms.RadioButton();
            this.rad_Answer3 = new System.Windows.Forms.RadioButton();
            this.rad_Answer2 = new System.Windows.Forms.RadioButton();
            this.rad_Answer1 = new System.Windows.Forms.RadioButton();
            this.lbl_answer1 = new System.Windows.Forms.Label();
            this.lbl_answer2 = new System.Windows.Forms.Label();
            this.lbl_answer3 = new System.Windows.Forms.Label();
            this.lbl_answer4 = new System.Windows.Forms.Label();
            this.btn_submit = new System.Windows.Forms.Button();
            this.GrpBx_AnswerSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Question
            // 
            this.lbl_Question.AutoSize = true;
            this.lbl_Question.Location = new System.Drawing.Point(13, 13);
            this.lbl_Question.Name = "lbl_Question";
            this.lbl_Question.Size = new System.Drawing.Size(52, 13);
            this.lbl_Question.TabIndex = 0;
            this.lbl_Question.Text = "Question:";
            // 
            // GrpBx_AnswerSelection
            // 
            this.GrpBx_AnswerSelection.Controls.Add(this.rad_Answer4);
            this.GrpBx_AnswerSelection.Controls.Add(this.rad_Answer3);
            this.GrpBx_AnswerSelection.Controls.Add(this.rad_Answer2);
            this.GrpBx_AnswerSelection.Controls.Add(this.rad_Answer1);
            this.GrpBx_AnswerSelection.Location = new System.Drawing.Point(16, 44);
            this.GrpBx_AnswerSelection.Name = "GrpBx_AnswerSelection";
            this.GrpBx_AnswerSelection.Size = new System.Drawing.Size(56, 214);
            this.GrpBx_AnswerSelection.TabIndex = 1;
            this.GrpBx_AnswerSelection.TabStop = false;
            // 
            // rad_Answer4
            // 
            this.rad_Answer4.AutoSize = true;
            this.rad_Answer4.Location = new System.Drawing.Point(22, 163);
            this.rad_Answer4.Name = "rad_Answer4";
            this.rad_Answer4.Size = new System.Drawing.Size(14, 13);
            this.rad_Answer4.TabIndex = 3;
            this.rad_Answer4.TabStop = true;
            this.rad_Answer4.UseVisualStyleBackColor = true;
            // 
            // rad_Answer3
            // 
            this.rad_Answer3.AutoSize = true;
            this.rad_Answer3.Location = new System.Drawing.Point(22, 117);
            this.rad_Answer3.Name = "rad_Answer3";
            this.rad_Answer3.Size = new System.Drawing.Size(14, 13);
            this.rad_Answer3.TabIndex = 2;
            this.rad_Answer3.TabStop = true;
            this.rad_Answer3.UseVisualStyleBackColor = true;
            // 
            // rad_Answer2
            // 
            this.rad_Answer2.AutoSize = true;
            this.rad_Answer2.Location = new System.Drawing.Point(22, 72);
            this.rad_Answer2.Name = "rad_Answer2";
            this.rad_Answer2.Size = new System.Drawing.Size(14, 13);
            this.rad_Answer2.TabIndex = 1;
            this.rad_Answer2.TabStop = true;
            this.rad_Answer2.UseVisualStyleBackColor = true;
            // 
            // rad_Answer1
            // 
            this.rad_Answer1.AutoSize = true;
            this.rad_Answer1.Location = new System.Drawing.Point(22, 28);
            this.rad_Answer1.Name = "rad_Answer1";
            this.rad_Answer1.Size = new System.Drawing.Size(14, 13);
            this.rad_Answer1.TabIndex = 0;
            this.rad_Answer1.TabStop = true;
            this.rad_Answer1.UseVisualStyleBackColor = true;
            // 
            // lbl_answer1
            // 
            this.lbl_answer1.AutoSize = true;
            this.lbl_answer1.Location = new System.Drawing.Point(90, 72);
            this.lbl_answer1.Name = "lbl_answer1";
            this.lbl_answer1.Size = new System.Drawing.Size(51, 13);
            this.lbl_answer1.TabIndex = 2;
            this.lbl_answer1.Text = "Answer 1";
            this.lbl_answer1.Click += new System.EventHandler(this.lbl_answer1_Click);
            // 
            // lbl_answer2
            // 
            this.lbl_answer2.AutoSize = true;
            this.lbl_answer2.Location = new System.Drawing.Point(90, 116);
            this.lbl_answer2.Name = "lbl_answer2";
            this.lbl_answer2.Size = new System.Drawing.Size(51, 13);
            this.lbl_answer2.TabIndex = 3;
            this.lbl_answer2.Text = "Answer 2";
            this.lbl_answer2.Click += new System.EventHandler(this.lbl_answer2_Click);
            // 
            // lbl_answer3
            // 
            this.lbl_answer3.AutoSize = true;
            this.lbl_answer3.Location = new System.Drawing.Point(90, 161);
            this.lbl_answer3.Name = "lbl_answer3";
            this.lbl_answer3.Size = new System.Drawing.Size(51, 13);
            this.lbl_answer3.TabIndex = 4;
            this.lbl_answer3.Text = "Answer 3";
            this.lbl_answer3.Click += new System.EventHandler(this.lbl_answer3_Click);
            // 
            // lbl_answer4
            // 
            this.lbl_answer4.AutoSize = true;
            this.lbl_answer4.Location = new System.Drawing.Point(90, 207);
            this.lbl_answer4.Name = "lbl_answer4";
            this.lbl_answer4.Size = new System.Drawing.Size(51, 13);
            this.lbl_answer4.TabIndex = 5;
            this.lbl_answer4.Text = "Answer 4";
            this.lbl_answer4.Click += new System.EventHandler(this.lbl_answer4_Click);
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(376, 256);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(75, 23);
            this.btn_submit.TabIndex = 6;
            this.btn_submit.Text = "Submit";
            this.btn_submit.UseVisualStyleBackColor = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // QuestionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 291);
            this.Controls.Add(this.btn_submit);
            this.Controls.Add(this.lbl_answer4);
            this.Controls.Add(this.lbl_answer3);
            this.Controls.Add(this.lbl_answer2);
            this.Controls.Add(this.lbl_answer1);
            this.Controls.Add(this.GrpBx_AnswerSelection);
            this.Controls.Add(this.lbl_Question);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "QuestionWindow";
            this.Text = "Questions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuestionWindow_FormClosing);
            this.GrpBx_AnswerSelection.ResumeLayout(false);
            this.GrpBx_AnswerSelection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Question;
        private System.Windows.Forms.GroupBox GrpBx_AnswerSelection;
        private System.Windows.Forms.RadioButton rad_Answer4;
        private System.Windows.Forms.RadioButton rad_Answer3;
        private System.Windows.Forms.RadioButton rad_Answer2;
        private System.Windows.Forms.RadioButton rad_Answer1;
        private System.Windows.Forms.Label lbl_answer1;
        private System.Windows.Forms.Label lbl_answer2;
        private System.Windows.Forms.Label lbl_answer3;
        private System.Windows.Forms.Label lbl_answer4;
        private System.Windows.Forms.Button btn_submit;

    }
}

