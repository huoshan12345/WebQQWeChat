using System;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    [Serializable]
    public class QQAccount : QQUser
    {
        public string Password { get; set; }
        public string Username { get; set; }

        public string Vfwebqq { get; set; }
    }
}
