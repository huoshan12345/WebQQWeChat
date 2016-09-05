namespace iQQ.Net.BatchHangQQ
{
    partial class FmAddQQ
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_QQNum = new System.Windows.Forms.TextBox();
            this.textBox2_QQPassword = new System.Windows.Forms.TextBox();
            this.cboLoginProtocol = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(31, 117);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(118, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "号码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码：";
            // 
            // textBox_QQNum
            // 
            this.textBox_QQNum.Location = new System.Drawing.Point(93, 25);
            this.textBox_QQNum.Name = "textBox_QQNum";
            this.textBox_QQNum.Size = new System.Drawing.Size(100, 21);
            this.textBox_QQNum.TabIndex = 4;
            // 
            // textBox2_QQPassword
            // 
            this.textBox2_QQPassword.Location = new System.Drawing.Point(93, 52);
            this.textBox2_QQPassword.Name = "textBox2_QQPassword";
            this.textBox2_QQPassword.Size = new System.Drawing.Size(100, 21);
            this.textBox2_QQPassword.TabIndex = 5;
            // 
            // cboLoginProtocol
            // 
            this.cboLoginProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoginProtocol.FormattingEnabled = true;
            this.cboLoginProtocol.Location = new System.Drawing.Point(93, 79);
            this.cboLoginProtocol.Name = "cboLoginProtocol";
            this.cboLoginProtocol.Size = new System.Drawing.Size(100, 20);
            this.cboLoginProtocol.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "协议：";
            // 
            // fmAddQQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 152);
            this.Controls.Add(this.cboLoginProtocol);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2_QQPassword);
            this.Controls.Add(this.textBox_QQNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.MaximizeBox = false;
            this.Name = "FmAddQQ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加一个QQ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_QQNum;
        private System.Windows.Forms.TextBox textBox2_QQPassword;
        private System.Windows.Forms.ComboBox cboLoginProtocol;
        private System.Windows.Forms.Label label3;
    }
}