using System;
using System.Linq;
using System.Windows.Forms;
using iQQ.Net.WebQQCore.Im.Bean;

namespace iQQ.Net.BatchHangQQ
{
    public partial class FmAddQQ : Form
    {
        public FmAddQQ()
        {
            InitializeComponent();
            cboLoginProtocol.Items.AddRange(QQClientType.QQClientAllTypes.Select(type => type.Value).Cast<object>().ToArray());
            cboLoginProtocol.SelectedIndex = 0;
        }

        public FmAddQQ(string qqNum, string qqPwd, string typeName):this()
        {
            textBox_QQNum.Text = qqNum;
            textBox2_QQPassword.Text = qqPwd;
            cboLoginProtocol.SelectedIndex = cboLoginProtocol.FindStringExact(typeName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            _addQQ(textBox_QQNum.Text, textBox2_QQPassword.Text, cboLoginProtocol.Text);
            Close();
        }

        public delegate void AddQQ(string qqNum, string qqPassword, string clientType);
        public AddQQ _addQQ;
    }
}
