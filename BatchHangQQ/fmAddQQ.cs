using System;
using System.Linq;
using System.Windows.Forms;
using iQQ.Net.WebQQCore.Im.Bean;

namespace iQQ.Net.BatchHangQQ
{
    public partial class fmAddQQ : Form
    {
        private static readonly string[] LoginProtocol = { };

        public fmAddQQ()
        {
            InitializeComponent();
            cboLoginProtocol.Items.AddRange(QQClientType.QQClientAllTypes.Select(type => type.Value).Cast<object>().ToArray());
            cboLoginProtocol.SelectedIndex = 0;
        }

        public fmAddQQ(string qqNum, string qqPwd, string typeName):this()
        {
            this.textBox_QQNum.Text = qqNum;
            this.textBox2_QQPassword.Text = qqPwd;
            cboLoginProtocol.SelectedIndex = cboLoginProtocol.FindStringExact(typeName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            addQQ(this.textBox_QQNum.Text, this.textBox2_QQPassword.Text, cboLoginProtocol.Text);
            this.Close();
        }

        public delegate void AddQQ(string qqNum, string qqPassword, string clientType);
        public AddQQ addQQ;
    }
}
