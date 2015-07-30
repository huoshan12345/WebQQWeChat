using System;
using System.Linq;
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
            cboLoginProtocol.Items.AddRange(LoginProtocol.Cast<object>().ToArray());
            cboLoginProtocol.SelectedIndex = 1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
