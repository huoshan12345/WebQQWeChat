using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Bean;
using FxUtility.Extensions;

namespace WebWeChat.Im.Util
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



        public static string ToJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static JToken ToJToken(this string str)
        {
            return JToken.Parse(str);
        }

        public static JObject ToJObject(this JToken token)
        {
            return token.ToObject<JObject>();
        }

        public static JArray ToJArray(this JToken token)
        {
            return token.ToObject<JArray>();
        }

        public static string ToSimpleString(this JToken obj)
        {
            return obj.ToString(Formatting.None);
        }

        public static int ToInt(this JToken token)
        {
            return token.ToObject<int>();
        }

        public static long ToLong(this JToken token)
        {
            return token.ToObject<long>();
        }

        public static T ToEnum<T>(this JToken token) where T : struct, IConvertible
        {
            return token.ToString().ToEnum<T>();
        }
    }
}
