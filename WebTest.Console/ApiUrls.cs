using System;
using System.Collections.Generic;
using System.Text;

namespace WebTest
{
    public static class ApiUrls
    {
        public const string HomeUrl = "http://localhost:9000";
        public const string GetTokenUrl = HomeUrl + "/connect/token";
        public const string LoginQQUrl = HomeUrl + "/QQ/LoginClient";
        public const string GetAndClearEvents = HomeUrl + "/QQ/GetAndClearEvents";
    }
}
