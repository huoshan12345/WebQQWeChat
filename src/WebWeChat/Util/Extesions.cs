using System;
using System.Collections.Generic;
using FclEx.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Event;

namespace WebWeChat.Util
{
    public static class Extesions
    {
        private static readonly HashSet<string> SpecialUsers = new HashSet<string>()
        {
            "newsapp", "fmessage", "filehelper", "weibo", "qqmail", "fmessage", "tmessage", "qmessage", "qqsync", "floatbottle",
            "lbsapp", "shakeapp", "medianote", "qqfriend", "readerapp", "blogapp", "facebookapp", "masssendapp", "meishiapp", "feedsapp",
            "voip", "blogappweixin", "weixin", "brandsessionholder", "weixinreminder", "wxid_novlwrv3lqwv11", "gh_22b87fa7cb3c", "officialaccounts",
            "notification_messages", "wxid_novlwrv3lqwv11", "gh_22b87fa7cb3c", "wxitil", "userexperience_alarm", "notification_messages"
        };

        public static bool IsSpecialUser(this ContactMember member)
        {
            return SpecialUsers.Contains(member.UserName);
        }

        public static bool IsPublicUsers(this ContactMember member)
        {
            return (member.VerifyFlag & 8) != 0;
        }

        public static bool IsGroup(this ContactMember member)
        {
            return member.UserName.StartsWith("@@");
        }

        public static T Get<T>(this WeChatNotifyEvent e)
        {
            return (T)e.Target;
        }
    }
}
