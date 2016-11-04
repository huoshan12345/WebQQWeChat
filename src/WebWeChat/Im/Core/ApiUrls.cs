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
        public const string WebwxInit = "{0}/webwxinit?pass_ticket={1}&skey={2}&r={3}";
        public const string StatusNotify = "{0}/webwxstatusnotify?lang=zh_CN&pass_ticket={1}";
        public const string GetContact = "{0}/webwxgetcontact?pass_ticket={1}&skey={2}&r={3}";
        public const string BatchGetContact = "{0}/webwxbatchgetcontact?type=ex&pass_ticket={1}&r={2}";
        // public const string SyncCheck = "{0}/synccheck";
        public const string WebwxSync = "{0}/webwxsync?sid={1}&skey={2}&lang=zh_CN&pass_ticket={3}";
        // public const string WebwxSync = "{0}/webwxsync";
        public const string SendMsg = "{0}/webwxsendmsg";


        public static readonly string[] SyncHosts =
        {
            "webpush.wx.qq.com",
            "webpush.wx2.qq.com",
            "webpush.wx8.qq.com",
            "webpush.web2.wechat.com",
            "webpush.web.wechat.com",
            //"webpush.wechat.com",
            //"webpush1.wechat.com",
            //"webpush2.wechat.com",
            //"webpush1.wechatapp.com",
            //"webpush.wechatapp.com",
        };

        public const string TulingRobot = "http://www.tuling123.com/openapi/api";
    }
}
