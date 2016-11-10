using FxUtility.Extensions;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean
{
    /// <summary>
    /// QQ好友
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// 头像
        /// </summary>
        public int Face { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public long Uin { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonProperty("lnick")]
        public string LongNick { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }

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

        public int VipLevel { get; set; } // 可以用QQUser.VipInfo代替

        public bool IsVip { get; set; }
    }
}
