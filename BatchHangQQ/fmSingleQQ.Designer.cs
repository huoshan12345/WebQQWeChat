namespace iQQ.Net.BatchHangQQ
{
    partial class fmSingleQQ
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbQQNum = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.Label();
            this.tbQQPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VerifyPic = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVerifyCode = new System.Windows.Forms.TextBox();
            this.cboLoginProtocol = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.VerifyPic)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(80, 340);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "QQ号码：";
            // 
            // tbQQNum
            // 
            this.tbQQNum.Location = new System.Drawing.Point(97, 59);
            this.tbQQNum.Name = "tbQQNum";
            this.tbQQNum.Size = new System.Drawing.Size(100, 21);
            this.tbQQNum.TabIndex = 2;
            // 
            // txt_pwd
            // 
            this.txt_pwd.AutoSize = true;
            this.txt_pwd.Location = new System.Drawing.Point(38, 106);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.Size = new System.Drawing.Size(53, 12);
            this.txt_pwd.TabIndex = 3;
            this.txt_pwd.Text = "QQ密码：";
            // 
            // tbQQPwd
            // 
            this.tbQQPwd.Location = new System.Drawing.Point(97, 103);
            this.tbQQPwd.Name = "tbQQPwd";
            this.tbQQPwd.PasswordChar = '*';
            this.tbQQPwd.Size = new System.Drawing.Size(100, 21);
            this.tbQQPwd.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "验证码：";
            // 
            // VerifyPic
            // 
            this.VerifyPic.Location = new System.Drawing.Point(97, 160);
            this.VerifyPic.Name = "VerifyPic";
            this.VerifyPic.Size = new System.Drawing.Size(127, 59);
            this.VerifyPic.TabIndex = 6;
            this.VerifyPic.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "输入验证码：";
            // 
            // tbVerifyCode
            // 
            this.tbVerifyCode.Location = new System.Drawing.Point(97, 231);
            this.tbVerifyCode.Name = "tbVerifyCode";
            this.tbVerifyCode.Size = new System.Drawing.Size(100, 21);
            this.tbVerifyCode.TabIndex = 8;
            // 
            // cboLoginProtocol
            // 
            this.cboLoginProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoginProtocol.Location = new System.Drawing.Point(97, 277);
            this.cboLoginProtocol.Name = "cboLoginProtocol";
            this.cboLoginProtocol.Size = new System.Drawing.Size(100, 20);
            this.cboLoginProtocol.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 280);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "登录协议：";
            // 
            // fmSingleQQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 406);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboLoginProtocol);
            this.Controls.Add(this.tbVerifyCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VerifyPic);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbQQPwd);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.tbQQNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Name = "fmSingleQQ";
            this.Text = "MyWebQQ";
            ((System.ComponentModel.ISupportInitialize)(this.VerifyPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbQQNum;
        private System.Windows.Forms.Label txt_pwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox VerifyPic;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVerifyCode;
        private System.Windows.Forms.TextBox tbQQPwd;
        private System.Windows.Forms.ComboBox cboLoginProtocol;
        private System.Windows.Forms.Label label4;
    }
}

