using System;
using System.Windows.Forms;

//using System.Data;

namespace iQQ.Net.BatchHangQQ
{
    public partial class fmSingleQQ : Form
    {
        private static readonly string[] LoginProtocol = { "WebQQ", "MobileQQ" };

        public fmSingleQQ()
        {
            InitializeComponent();
            cboLoginProtocol.Items.AddRange(LoginProtocol);
            cboLoginProtocol.SelectedIndex = 1;
            tbQQNum.Text = "459734234";
            tbQQPwd.Text = "lijing12345";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
