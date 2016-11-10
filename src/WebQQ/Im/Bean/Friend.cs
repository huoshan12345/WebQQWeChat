using FxUtility.Extensions;

namespace WebQQ.Im.Bean
{

    /// <summary>
    /// QQ好友，出现在好友列表的用户
    /// </summary>
    
    public class Friend : QQUser
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }

        public int MarknameType { get; set; }

        public int CategoryIndex { get; set; }

        public int InfoFlag { get; set; }

        public int ClientType { get; set; }

        public QQStatusType Status { get; set; }

        public string ShowName => MarkName.IsNullOrEmpty() ? (Nick.IsNullOrEmpty() ? Uin.ToString() : Nick) : MarkName;

        // public int VipLevel { get; set; } // 可以用QQUser.VipInfo代替

        public bool IsVip { get; set; }
    }
}
