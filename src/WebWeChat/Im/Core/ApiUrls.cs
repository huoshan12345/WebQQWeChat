using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWeChat.Im.Core
{
    public static class ApiUrls
    {
        public const string Appid = "wx782c26e4c19acffb";
        public const string GetUuid = "https://login.weixin.qq.com/jslogin";
        public const string GetQRCode = "https://login.weixin.qq.com/qrcode/{0}";
        public const string CheckQRCode = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login";
        public const string WebwxInit = "{0}/webwxinit";
        public const string StatusNotify = "{0}/webwxstatusnotify";
        public const string GetContact = "{0}/webwxgetcontact";
        public const string BatchGetContact = "{0}/webwxbatchgetcontact";
        public const string SyncCheck = "{0}/synccheck";

        public static readonly string[] SyncHosts =
        {
            "webpush.wx.qq.com",
            "webpush.wx2.qq.com",
            "webpush.weixin.qq.com",
            "webpush2.weixin.qq.com",
            //"webpush.wechat.com",
            //"webpush1.wechat.com",
            //"webpush2.wechat.com",
            //"webpush1.wechatapp.com",
            //"webpush.wechatapp.com",
        };
    }
}
