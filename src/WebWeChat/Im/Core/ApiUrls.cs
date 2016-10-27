using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWeChat.Im.Core
{
    public abstract class ApiUrls
    {
        public const string Appid = "wx782c26e4c19acffb";
        public const string GetUuid = "https://login.weixin.qq.com/jslogin";
        public const string GetQRCode = "https://login.weixin.qq.com/qrcode/{0}";
        public const string CheckQRCode = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login";
    }
}
