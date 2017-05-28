using FclEx.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using HttpAction.Event;
using Newtonsoft.Json;

namespace WebQQ.Im.Event
{
    /// <summary>
    /// QQ通知事件类型
    /// </summary>
    [Description("QQ通知事件类型")]
    public enum QQNotifyEventType
    {
        NeedUpdateGroups = -2,
        
        NeedUpdateFriends = -1,

        /// <summary>
        /// 登录成功
        /// </summary>
        LoginSuccess = 0,
        /** 重新登录成功 **/
        ReloginSuccess,
        /** 网络连接出错，客户端已经掉线 */
        NetError,
        /** 未知错误，如retcode多次出现未知值 */
        UnknownError,
        /** 客户端被踢下线，可能是其他地方登陆 */
        KickOffline,
        /** 对方正在输入 */
        BuddyInput,
        /** 窗口震动 */
        ShakeWindow,

        /// <summary>
        /// 聊天消息，包括好友，临时会话
        /// </summary>
        [Description("聊天消息")]
        ChatMsg,

        /// <summary>
        /// 群，讨论组消息
        /// </summary>
        [Description("群和讨论组消息")]
        GroupMsg,

        /** 好友通知，如其他人请求添加好友，添加其他用户请求通过或者拒绝 */
        BuddyNotify,
        /** 群通知，如管理员通过或拒绝了添加群请求，群成员退出等 */
        GroupNotify,
        /** 文件传输通知，如对方请求发送文件，对方已同意接受文件等 */
        FileNotify,
        /** 视频通知，如对方请求和你视频，对方同意视频等。。 */
        AvNotify,
        /** 系统广播 */
        SystemNotify,
        /** 好友状态改变 */
        BuddyStatusChange,
        /** 验证请求，需要用户输入验证码以继续 */
        CapachaVerify,
        /** 新邮件通知 */
        EmailNotify,

        /// <summary>
        /// 二维码已就绪
        /// </summary>
        [Description("二维码已就绪")]
        QRCodeReady,

        /// <summary>
        /// 二维码失效
        /// </summary>
        [Description("二维码失效")]
        QRCodeInvalid,

        /// <summary>
        /// 二维码验证成功
        /// </summary>
        [Description("二维码验证成功")]
        QRCodeSuccess,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
    }

    public class QQNotifyEvent : EventArgs
    {
        private static readonly ConcurrentDictionary<QQNotifyEventType, QQNotifyEvent> EmptyEvents
            = new ConcurrentDictionary<QQNotifyEventType, QQNotifyEvent>();

        public QQNotifyEventType Type { get; }
        public object Target { get; }

        [JsonConstructor]
        private QQNotifyEvent(QQNotifyEventType type, object target = null)
        {
            Type = type;
            Target = target;
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }

        public static QQNotifyEvent CreateEvent(QQNotifyEventType type, object target = null)
        {
            return target == null ? EmptyEvents.GetOrAdd(type, key => new QQNotifyEvent(key)) : new QQNotifyEvent(type, target);
        }
    }
}
