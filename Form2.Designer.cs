
namespace SmartFalcon
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServerID = new System.Windows.Forms.TextBox();
            this.tbChannelID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbContent = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "チャンネルID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "サーバーID";
            // 
            // tbServerID
            // 
            this.tbServerID.Location = new System.Drawing.Point(14, 24);
            this.tbServerID.Name = "tbServerID";
            this.tbServerID.Size = new System.Drawing.Size(442, 19);
            this.tbServerID.TabIndex = 2;
            // 
            // tbChannelID
            // 
            this.tbChannelID.Location = new System.Drawing.Point(12, 82);
            this.tbChannelID.Name = "tbChannelID";
            this.tbChannelID.Size = new System.Drawing.Size(442, 19);
            this.tbChannelID.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "投稿テキスト";
            // 
            // tbContent
            // 
            this.tbContent.Location = new System.Drawing.Point(12, 147);
            this.tbContent.Multiline = true;
            this.tbContent.Name = "tbContent";
            this.tbContent.Size = new System.Drawing.Size(442, 209);
            this.tbContent.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(14, 362);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(440, 34);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 408);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbContent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbChannelID);
            this.Controls.Add(this.tbServerID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbServerID;
        public System.Windows.Forms.TextBox tbChannelID;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbContent;
        public System.Windows.Forms.Button btnSend;
    }
}