using System;
using System.Linq;
using System.Windows.Forms;

//using System.Data;

namespace iQQ.Net.BatchHangQQ
{
    public partial class FmSingleQQ : Form
    {
        private static readonly string[] _loginProtocol = { "WebQQ", "MobileQQ" };

        public FmSingleQQ()
        {
            InitializeComponent();
            cboLoginProtocol.Items.AddRange(_loginProtocol.Cast<object>().ToArray());
            cboLoginProtocol.SelectedIndex = 1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
