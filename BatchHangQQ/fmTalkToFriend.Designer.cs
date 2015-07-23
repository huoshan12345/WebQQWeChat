namespace iQQ.Net.BatchHangQQ
{
    partial class fmTalkToFriend
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
            this.txt_client = new System.Windows.Forms.TextBox();
            this.sendmsg = new System.Windows.Forms.TextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lab_friend_num = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_client
            // 
            this.txt_client.Location = new System.Drawing.Point(13, 20);
            this.txt_client.Multiline = true;
            this.txt_client.Name = "txt_client";
            this.txt_client.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_client.Size = new System.Drawing.Size(297, 235);
            this.txt_client.TabIndex = 0;
            // 
            // sendmsg
            // 
            this.sendmsg.Location = new System.Drawing.Point(12, 274);
            this.sendmsg.Multiline = true;
            this.sendmsg.Name = "sendmsg";
            this.sendmsg.Size = new System.Drawing.Size(297, 82);
            this.sendmsg.TabIndex = 1;
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(331, 334);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(62, 21);
            this.btSend.TabIndex = 0;
            this.btSend.Text = "发送";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(318, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Talk To:";
            // 
            // lab_friend_num
            // 
            this.lab_friend_num.AutoSize = true;
            this.lab_friend_num.Location = new System.Drawing.Point(318, 102);
            this.lab_friend_num.Name = "lab_friend_num";
            this.lab_friend_num.Size = new System.Drawing.Size(0, 12);
            this.lab_friend_num.TabIndex = 4;
            // 
            // fmTalkToFriend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 382);
            this.Controls.Add(this.lab_friend_num);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.sendmsg);
            this.Controls.Add(this.txt_client);
            this.Name = "fmTalkToFriend";
            this.Text = "fmTalkToFriend";
            this.Load += new System.EventHandler(this.TalkToFriend_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.closeTalk);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txt_client;
        private System.Windows.Forms.TextBox sendmsg;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lab_friend_num;
    }
}