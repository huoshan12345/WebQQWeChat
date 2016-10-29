using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWeChat.Im.Core
{
    public enum SyncCheckResult
    {
        /// <summary>
        /// 
        /// </summary>
        Nothing,

        /// <summary>
        /// 新消息
        /// </summary>
        NewMsg,

        /// <summary>
        /// 正在使用手机微信
        /// </summary>
        UsingPhone,

        /// <summary>
        /// 红包
        /// </summary>
        RedEnvelope,

        /// <summary>
        /// 已离线
        /// </summary>
        Offline,

        /// <summary>
        /// 被踢
        /// </summary>
        Kicked,
    }
}
