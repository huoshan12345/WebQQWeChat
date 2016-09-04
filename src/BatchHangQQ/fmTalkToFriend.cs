using System;
using System.Windows.Forms;

namespace iQQ.Net.BatchHangQQ
{
    public partial class fmTalkToFriend : Form
    {
        public fmQQList parent;
        public  string qqNum = "";

        public fmTalkToFriend()
        {
            InitializeComponent();
        }

        private void TalkToFriend_Load(object sender, EventArgs e)
        {
            this.lab_friend_num.Text = qqNum;
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            /* 发送消息给好友 */

        }

        /// <summary>
        /// 关闭聊天窗口时移除聊天列表及窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeTalk(object sender, FormClosedEventArgs e)
        {

        }
    }
}
