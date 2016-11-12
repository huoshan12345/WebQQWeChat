using FxUtility.Extensions;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    /// <summary>
    /// QQ好友
    /// </summary>
    public class QQFriend : FriendInfo
    {
        public int InfoFlag { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string LongNick { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }

        public int MarknameType { get; set; }

        public int CategoryIndex { get; set; }

        public int ClientType { get; set; }

        public int VipLevel { get; set; }

        public int IsVip { get; set; }

        public long QQNumber { get; set; }

        public string ShowName => MarkName.IsNullOrEmpty() ? (Nick.IsNullOrEmpty() ? Uin.ToString() : Nick) : MarkName;
    }
}
