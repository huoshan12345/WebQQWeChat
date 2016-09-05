using System;
using System.Windows.Forms;

namespace iQQ.Net.BatchHangQQ
{
    public partial class FmTalkToFriend : Form
    {
        public FmQQList _parent;
        public  string _qqNum = "";

        public FmTalkToFriend()
        {
            InitializeComponent();
        }

        private void TalkToFriend_Load(object sender, EventArgs e)
        {
            this.lab_friend_num.Text = _qqNum;
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
        private void CloseTalk(object sender, FormClosedEventArgs e)
        {

        }
    }
}
