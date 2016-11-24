using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        [EnumMember(Value = "input_notify")]
        InputNotify,

        /// <summary>
        /// 好友消息
        /// </summary>
        [EnumMember(Value = "message")]
        Message,

        /// <summary>
        /// 群消息
        /// </summary>
        [EnumMember(Value = "group_message")]
        GroupMessage,

        /// <summary>
        /// 讨论组消息
        /// </summary>
        [EnumMember(Value = "discu_message")]
        DiscussionMessage,

        /// <summary>
        /// 临时会话消息
        /// </summary>
        [EnumMember(Value = "sess_message")]
        SessionMessage,

        /// <summary>
        /// 窗口震动
        /// </summary>
        [EnumMember(Value = "shake_message")]
        ShakeMessage,

        /// <summary>
        /// 被踢下线
        /// </summary>
        [EnumMember(Value = "kick_message")]
        KickMessage,

        /// <summary>
        /// 群成员状态变动
        /// </summary>
        [EnumMember(Value = "buddies_status_change")]
        BuddiesStatusChange,

        /// <summary>
        /// 系统消息，好友添加
        /// </summary>
        [EnumMember(Value = "system_message")]
        SystemMessage,

        /// <summary>
        /// 发布了共享文件
        /// </summary>
        [EnumMember(Value = "group_web_message")]
        GroupWebMessage,

        /// <summary>
        /// 被踢出了群
        /// </summary>
        [EnumMember(Value = "sys_g_msg")]
        SysGroupMsg,
    }
}
