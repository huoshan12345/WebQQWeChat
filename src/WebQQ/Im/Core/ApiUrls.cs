using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebQQ.Im.Core
{
    public static class ApiUrls
    {
        public const string GetQRCode = "https://ssl.ptlogin2.qq.com/ptqrshow";
        public const string CheckQRCode = "https://ssl.ptlogin2.qq.com/ptqrlogin";
        public const string GetVfwebqq = "http://s.web2.qq.com/api/getvfwebqq";
        public const string ChannelLogin = "http://d1.web2.qq.com/channel/login2";
        public const string GetFriends = "http://s.web2.qq.com/api/get_user_friends2";
        public const string Referrer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
        public const string ReferrerS = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        public const string GetGroupNameList = "http://s.web2.qq.com/api/get_group_name_list_mask2";
        public const string GetDiscussionList = "http://s.web2.qq.com/api/get_discus_list";
        public const string GetSelfInfo = "http://s.web2.qq.com/api/get_self_info2";
        public const string GetOnlineFriends = "http://d1.web2.qq.com/channel/get_online_buddies2";
        public const string GetFriendLongNick = "http://s.web2.qq.com/api/get_single_long_nick";
        public const string SendFriendMsg = "https://d1.web2.qq.com/channel/send_buddy_msg2";
        public const string SendGroupMsg = "https://d1.web2.qq.com/channel/send_qun_msg2";
    }
}
