using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FclEx.Extensions;

namespace WebWeChat.Im.Bean
{
    public class ContactMember : GroupMember
    {
        /// <summary>
        /// 头像图片链接地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ContactFlag { get; set; }
        
        /// <summary>
        /// 成员数量，只有在群组信息中才有效 
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// 成员列表，只有在群组信息中才有效
        /// </summary>
        public List<GroupMember> MemberList { get; set; }

        /// <summary>
        /// 备注名称
        /// </summary>
        public string RemarkName { get; set; }

        public int HideInputBarFlag { get; set; }

        /// <summary>
        /// 性别，0-未设置（公众号、保密），1-男，2-女
        /// </summary>
        public int Sex { get; set; }


        public string Signature { get; set; }
        public int VerifyFlag { get; set; }
        public long OwnerUin { get; set; }

        /// <summary>
        /// 是否为星标朋友 0-否 1-是
        /// </summary>
        public int StarFriend { get; set; }
        public int AppAccountFlag { get; set; }
        public int Statues { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        public string Alias { get; set; }
        public int SnsFlag { get; set; }
        public int UniFriend { get; set; }
        public int ChatRoomId { get; set; }
        public string EncryChatRoomId { get; set; }

        public override string ShowName => RemarkName.IsNullOrEmpty() ? (NickName.IsNullOrEmpty() ? UserName : NickName) : RemarkName;
    }

}
