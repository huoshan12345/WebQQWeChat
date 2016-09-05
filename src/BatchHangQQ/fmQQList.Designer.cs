namespace iQQ.Net.BatchHangQQ
{
    partial class FmQQList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmQQList));
            this.lvQQList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboLoginProtocol = new System.Windows.Forms.ComboBox();
            this.cboLoginStatus = new System.Windows.Forms.ComboBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboVerifyCodeDigit = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_SkipVerifyCode = new System.Windows.Forms.CheckBox();
            this.pbVerifyPic = new System.Windows.Forms.PictureBox();
            this.tbVerifyCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUseRobot = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.chkAutoReply = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnImportQQlist = new System.Windows.Forms.Button();
            this.btnExportQQlist = new System.Windows.Forms.Button();
            this.btnClearQQlist = new System.Windows.Forms.Button();
            this.btnAddQQ = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyPic)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvQQList
            // 
            this.lvQQList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvQQList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7});
            this.lvQQList.FullRowSelect = true;
            this.lvQQList.GridLines = true;
            this.lvQQList.HideSelection = false;
            this.lvQQList.Location = new System.Drawing.Point(12, 62);
            this.lvQQList.Name = "lvQQList";
            this.lvQQList.Size = new System.Drawing.Size(516, 399);
            this.lvQQList.TabIndex = 0;
            this.lvQQList.UseCompatibleStateImageBehavior = false;
            this.lvQQList.View = System.Windows.Forms.View.Details;
            this.lvQQList.DoubleClick += new System.EventHandler(this.lvQQList_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "号码";
            this.columnHeader2.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "密码";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "协议";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "等级";
            this.columnHeader5.Width = 40;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "剩余天数";
            this.columnHeader7.Width = 75;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboLoginProtocol);
            this.groupBox1.Controls.Add(this.cboLoginStatus);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(534, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 110);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录";
            // 
            // cboLoginProtocol
            // 
            this.cboLoginProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoginProtocol.FormattingEnabled = true;
            this.cboLoginProtocol.Location = new System.Drawing.Point(72, 48);
            this.cboLoginProtocol.Name = "cboLoginProtocol";
            this.cboLoginProtocol.Size = new System.Drawing.Size(68, 20);
            this.cboLoginProtocol.TabIndex = 5;
            // 
            // cboLoginStatus
            // 
            this.cboLoginStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoginStatus.FormattingEnabled = true;
            this.cboLoginStatus.Location = new System.Drawing.Point(72, 21);
            this.cboLoginStatus.Name = "cboLoginStatus";
            this.cboLoginStatus.Size = new System.Drawing.Size(68, 20);
            this.cboLoginStatus.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(8, 79);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(132, 24);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "登录方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "登录状态：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboVerifyCodeDigit);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.checkBox_SkipVerifyCode);
            this.groupBox2.Controls.Add(this.pbVerifyPic);
            this.groupBox2.Controls.Add(this.tbVerifyCode);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(534, 178);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 124);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "验证码";
            // 
            // cboVerifyCodeDigit
            // 
            this.cboVerifyCodeDigit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVerifyCodeDigit.FormattingEnabled = true;
            this.cboVerifyCodeDigit.Location = new System.Drawing.Point(40, 100);
            this.cboVerifyCodeDigit.Name = "cboVerifyCodeDigit";
            this.cboVerifyCodeDigit.Size = new System.Drawing.Size(40, 20);
            this.cboVerifyCodeDigit.TabIndex = 7;
            this.cboVerifyCodeDigit.SelectedIndexChanged += new System.EventHandler(this.tbVerifyCode_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "位数：";
            // 
            // checkBox_SkipVerifyCode
            // 
            this.checkBox_SkipVerifyCode.AutoSize = true;
            this.checkBox_SkipVerifyCode.Location = new System.Drawing.Point(92, 102);
            this.checkBox_SkipVerifyCode.Name = "checkBox_SkipVerifyCode";
            this.checkBox_SkipVerifyCode.Size = new System.Drawing.Size(48, 16);
            this.checkBox_SkipVerifyCode.TabIndex = 5;
            this.checkBox_SkipVerifyCode.Text = "跳过";
            this.checkBox_SkipVerifyCode.UseVisualStyleBackColor = true;
            // 
            // pbVerifyPic
            // 
            this.pbVerifyPic.Location = new System.Drawing.Point(8, 43);
            this.pbVerifyPic.Name = "pbVerifyPic";
            this.pbVerifyPic.Size = new System.Drawing.Size(132, 53);
            this.pbVerifyPic.TabIndex = 4;
            this.pbVerifyPic.TabStop = false;
            // 
            // tbVerifyCode
            // 
            this.tbVerifyCode.Location = new System.Drawing.Point(59, 14);
            this.tbVerifyCode.MaxLength = 5;
            this.tbVerifyCode.Name = "tbVerifyCode";
            this.tbVerifyCode.Size = new System.Drawing.Size(81, 21);
            this.tbVerifyCode.TabIndex = 3;
            this.tbVerifyCode.TextChanged += new System.EventHandler(this.tbVerifyCode_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "验证码：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(534, 399);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(146, 62);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "IP信息";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "地区：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "IP：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkUseRobot);
            this.groupBox4.Controls.Add(this.checkBox4);
            this.groupBox4.Controls.Add(this.checkBox3);
            this.groupBox4.Controls.Add(this.chkAutoReply);
            this.groupBox4.Location = new System.Drawing.Point(542, 308);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(146, 85);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "设置";
            // 
            // chkUseRobot
            // 
            this.chkUseRobot.AutoSize = true;
            this.chkUseRobot.Location = new System.Drawing.Point(80, 20);
            this.chkUseRobot.Name = "chkUseRobot";
            this.chkUseRobot.Size = new System.Drawing.Size(60, 16);
            this.chkUseRobot.TabIndex = 8;
            this.chkUseRobot.Text = "机器人";
            this.chkUseRobot.UseVisualStyleBackColor = true;
            this.chkUseRobot.CheckedChanged += new System.EventHandler(this.chkUseRobot_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(8, 64);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(36, 16);
            this.checkBox4.TabIndex = 6;
            this.checkBox4.Text = "xx";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(8, 42);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(96, 16);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "显示星号密码";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // chkAutoReply
            // 
            this.chkAutoReply.AutoSize = true;
            this.chkAutoReply.Location = new System.Drawing.Point(8, 20);
            this.chkAutoReply.Name = "chkAutoReply";
            this.chkAutoReply.Size = new System.Drawing.Size(72, 16);
            this.chkAutoReply.TabIndex = 6;
            this.chkAutoReply.Text = "自动回复";
            this.chkAutoReply.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(534, 467);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(146, 87);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "作者信息";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "QQ：89009143";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "作者：月光双刀";
            // 
            // btnImportQQlist
            // 
            this.btnImportQQlist.Location = new System.Drawing.Point(12, 12);
            this.btnImportQQlist.Name = "btnImportQQlist";
            this.btnImportQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnImportQQlist.TabIndex = 7;
            this.btnImportQQlist.Text = "导入列表";
            this.btnImportQQlist.UseVisualStyleBackColor = true;
            this.btnImportQQlist.Click += new System.EventHandler(this.btnImportQQlist_Click);
            // 
            // btnExportQQlist
            // 
            this.btnExportQQlist.Location = new System.Drawing.Point(132, 12);
            this.btnExportQQlist.Name = "btnExportQQlist";
            this.btnExportQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnExportQQlist.TabIndex = 8;
            this.btnExportQQlist.Text = "导出列表";
            this.btnExportQQlist.UseVisualStyleBackColor = true;
            this.btnExportQQlist.Click += new System.EventHandler(this.btnExportQQlist_Click);
            // 
            // btnClearQQlist
            // 
            this.btnClearQQlist.Location = new System.Drawing.Point(247, 12);
            this.btnClearQQlist.Name = "btnClearQQlist";
            this.btnClearQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnClearQQlist.TabIndex = 9;
            this.btnClearQQlist.Text = "清空列表";
            this.btnClearQQlist.UseVisualStyleBackColor = true;
            this.btnClearQQlist.Click += new System.EventHandler(this.btnClearQQlist_Click);
            // 
            // btnAddQQ
            // 
            this.btnAddQQ.Location = new System.Drawing.Point(358, 12);
            this.btnAddQQ.Name = "btnAddQQ";
            this.btnAddQQ.Size = new System.Drawing.Size(70, 35);
            this.btnAddQQ.TabIndex = 10;
            this.btnAddQQ.Text = "手动添加";
            this.btnAddQQ.UseVisualStyleBackColor = true;
            this.btnAddQQ.Click += new System.EventHandler(this.btnAddQQ_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMessage.Location = new System.Drawing.Point(12, 467);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.Size = new System.Drawing.Size(516, 87);
            this.tbMessage.TabIndex = 11;
            this.tbMessage.Text = "";
            // 
            // fmQQList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.btnAddQQ);
            this.Controls.Add(this.btnClearQQlist);
            this.Controls.Add(this.btnExportQQlist);
            this.Controls.Add(this.btnImportQQlist);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lvQQList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 600);
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "FmQQList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyWebQQ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmQQList_FormClosing);
            this.Load += new System.EventHandler(this.fmQQList_Load);
            this.SizeChanged += new System.EventHandler(this.fmQQList_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyPic)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvQQList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLoginProtocol;
        private System.Windows.Forms.ComboBox cboLoginStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_SkipVerifyCode;
        private System.Windows.Forms.PictureBox pbVerifyPic;
        private System.Windows.Forms.TextBox tbVerifyCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox chkAutoReply;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnImportQQlist;
        private System.Windows.Forms.Button btnExportQQlist;
        private System.Windows.Forms.Button btnClearQQlist;
        private System.Windows.Forms.Button btnAddQQ;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ComboBox cboVerifyCodeDigit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox tbMessage;
        private System.Windows.Forms.CheckBox chkUseRobot;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}