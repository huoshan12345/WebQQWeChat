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
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUseRobot = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.RichTextBox();
            this.btnAddQQ = new System.Windows.Forms.Button();
            this.btnClearQQlist = new System.Windows.Forms.Button();
            this.btnExportQQlist = new System.Windows.Forms.Button();
            this.btnImportQQlist = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
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
            this.columnHeader6,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7});
            this.lvQQList.FullRowSelect = true;
            this.lvQQList.GridLines = true;
            this.lvQQList.HideSelection = false;
            this.lvQQList.Location = new System.Drawing.Point(12, 62);
            this.lvQQList.Name = "lvQQList";
            this.lvQQList.Size = new System.Drawing.Size(516, 378);
            this.lvQQList.TabIndex = 0;
            this.lvQQList.UseCompatibleStateImageBehavior = false;
            this.lvQQList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "号码";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "类型";
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
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Location = new System.Drawing.Point(534, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 55);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(8, 20);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(124, 24);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbQRCode);
            this.groupBox2.Location = new System.Drawing.Point(534, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(138, 158);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "二维码";
            // 
            // pbQRCode
            // 
            this.pbQRCode.Location = new System.Drawing.Point(8, 20);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(124, 124);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbQRCode.TabIndex = 4;
            this.pbQRCode.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(534, 378);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(138, 62);
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
            this.groupBox4.Location = new System.Drawing.Point(534, 287);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(138, 85);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "设置";
            // 
            // chkUseRobot
            // 
            this.chkUseRobot.AutoSize = true;
            this.chkUseRobot.Location = new System.Drawing.Point(8, 20);
            this.chkUseRobot.Name = "chkUseRobot";
            this.chkUseRobot.Size = new System.Drawing.Size(60, 16);
            this.chkUseRobot.TabIndex = 8;
            this.chkUseRobot.Text = "机器人";
            this.chkUseRobot.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(534, 446);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(138, 108);
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
            // tbMessage
            // 
            this.tbMessage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMessage.Location = new System.Drawing.Point(12, 446);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.Size = new System.Drawing.Size(516, 108);
            this.tbMessage.TabIndex = 11;
            this.tbMessage.Text = "";
            this.tbMessage.WordWrap = false;
            // 
            // btnAddQQ
            // 
            this.btnAddQQ.Location = new System.Drawing.Point(358, 12);
            this.btnAddQQ.Name = "btnAddQQ";
            this.btnAddQQ.Size = new System.Drawing.Size(70, 35);
            this.btnAddQQ.TabIndex = 10;
            this.btnAddQQ.Text = "手动添加";
            this.btnAddQQ.UseVisualStyleBackColor = true;
            // 
            // btnClearQQlist
            // 
            this.btnClearQQlist.Location = new System.Drawing.Point(247, 12);
            this.btnClearQQlist.Name = "btnClearQQlist";
            this.btnClearQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnClearQQlist.TabIndex = 9;
            this.btnClearQQlist.Text = "清空列表";
            this.btnClearQQlist.UseVisualStyleBackColor = true;
            // 
            // btnExportQQlist
            // 
            this.btnExportQQlist.Location = new System.Drawing.Point(132, 12);
            this.btnExportQQlist.Name = "btnExportQQlist";
            this.btnExportQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnExportQQlist.TabIndex = 8;
            this.btnExportQQlist.Text = "导出列表";
            this.btnExportQQlist.UseVisualStyleBackColor = true;
            // 
            // btnImportQQlist
            // 
            this.btnImportQQlist.Location = new System.Drawing.Point(12, 12);
            this.btnImportQQlist.Name = "btnImportQQlist";
            this.btnImportQQlist.Size = new System.Drawing.Size(70, 35);
            this.btnImportQQlist.TabIndex = 7;
            this.btnImportQQlist.Text = "导入列表";
            this.btnImportQQlist.UseVisualStyleBackColor = true;
            // 
            // FmQQList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 561);
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
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
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
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.RichTextBox tbMessage;
        private System.Windows.Forms.CheckBox chkUseRobot;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button btnAddQQ;
        private System.Windows.Forms.Button btnClearQQlist;
        private System.Windows.Forms.Button btnExportQQlist;
        private System.Windows.Forms.Button btnImportQQlist;
    }
}