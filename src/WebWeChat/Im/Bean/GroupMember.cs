using Newtonsoft.Json;
using FxUtility.Extensions;

namespace WebWeChat.Im.Bean
{
    public class GroupMember
    {
        public int Uin { get; set; }

        /// <summary>
        ///  用户名称，一个"@"为好友，两个"@"为群组
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        public long AttrStatus { get; set; }

        /// <summary>
        /// 用户名拼音缩写
        /// </summary>
        [JsonProperty(PropertyName = "PYInitial")]
        public string PyInitial { get; set; }

        /// <summary>
        /// 用户名拼音全拼
        /// </summary>
        [JsonProperty(PropertyName = "PYQuanPin")]
        public string PyQuanPin { get; set; }

        /// <summary>
        /// 备注拼音缩写
        /// </summary>
        [JsonProperty(PropertyName = "RemarkPYInitial")]
        public string RemarkPyInitial { get; set; }

        /// <summary>
        /// 备注拼音全拼
        /// </summary>
        [JsonProperty(PropertyName = "RemarkPYQuanPin")]
        public string RemarkPyQuanPin { get; set; }

        public int MemberStatus { get; set; }

        public string DisplayName { get; set; }

        public string KeyWord { get; set; }

        public virtual string ShowName => DisplayName.IsNullOrEmpty() ? (NickName.IsNullOrEmpty() ? UserName : DisplayName) : NickName;
    }
}
