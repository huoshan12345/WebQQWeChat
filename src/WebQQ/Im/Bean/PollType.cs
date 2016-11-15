using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebQQ.Im.Bean
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PollType
    {
        /// <summary>
        /// 正在输入
        /// </summary>
        [JsonProperty("input_notify")]
        InputNotify,

        /// <summary>
        /// 好友消息
        /// </summary>
        [JsonProperty("message")]
        Message,

        /// <summary>
        /// 群消息
        /// </summary>
        [JsonProperty("group_message")]
        GroupMessage,

        /// <summary>
        /// 讨论组消息
        /// </summary>
        [JsonProperty("discu_message")]
        DiscussionMessage,

        /// <summary>
        /// 临时会话消息
        /// </summary>
        [JsonProperty("sess_message")]
        SessionMessage,

        /// <summary>
        /// 窗口震动
        /// </summary>
        [JsonProperty("shake_message")]
        ShakeMessage,

        /// <summary>
        /// 被踢下线
        /// </summary>
        [JsonProperty("kick_message")]
        KickMessage,

        /// <summary>
        /// 群成员状态变动
        /// </summary>
        [JsonProperty("buddies_status_change")]
        BuddiesStatusChange,

        /// <summary>
        /// 系统消息，好友添加
        /// </summary>
        [JsonProperty("system_message")]
        SystemMessage,

        /// <summary>
        /// 发布了共享文件
        /// </summary>
        [JsonProperty("group_web_message")]
        GroupWebMessage,

        /// <summary>
        /// 被踢出了群
        /// </summary>
        [JsonProperty("sys_g_msg")]
        SysGroupMsg,
    }
}
