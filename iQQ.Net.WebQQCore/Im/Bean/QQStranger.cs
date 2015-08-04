using System;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    [Serializable]
    public class QQStranger : QQUser
    {
        /// <summary>
        /// 临时会话信道
        /// </summary>
        public string GroupSig { get; set; }

        public int ServiceType { get; set; }

        /// <summary>
        /// 加好友时需要的参数
        /// </summary>
        public string Token { get; set; }
    }
}
