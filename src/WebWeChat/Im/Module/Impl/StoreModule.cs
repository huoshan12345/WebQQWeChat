using System.Collections.Generic;
using WebWeChat.Im.Bean;

namespace WebWeChat.Im.Module.Impl
{
    public class StoreModule : AbstractModule
    {
        /// <summary>
        /// 联系人总数
        /// </summary>
        public int MemberCount { get; set; }

        ///// <summary>
        ///// 公众号，服务号
        ///// </summary>
        //public List<Member> PublicUsersList { get; set; }

        ///// <summary>
        ///// 群
        ///// </summary>
        //public List<Member> GroupList { get; set; }

        ///// <summary>
        ///// 好友
        ///// </summary>
        //public List<Member> FriendList { get; set; }

        /// <summary>
        /// 主键是member的username
        /// </summary>
        public Dictionary<string, Member> MemberDic { get; set; }
    }
}
