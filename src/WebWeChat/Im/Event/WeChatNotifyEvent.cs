using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Extensions;

namespace WebWeChat.Im.Event
{    /// <summary>
     /// QQ通知事件类型
     /// </summary>
    [Description("微信通知事件类型")]
    public enum WeChatNotifyEventType
    {
        /// <summary>
        /// 二维码已就绪
        /// </summary>
        [Description("二维码已就绪")]
        QRcodeReady,

        /// <summary>
        /// 二维码失效
        /// </summary>
        [Description("二维码失效")]
        QRcodeInvalid,

        /// <summary>
        /// 二维码验证成功
        /// </summary>
        [Description("二维码验证成功")]
        QRcodeSuccess
    }

    public class WeChatNotifyEvent : EventArgs
    {
        public WeChatNotifyEventType Type { get; }
        public object Target { get; }

        public WeChatNotifyEvent(WeChatNotifyEventType type, object target)
        {
            Type = type;
            Target = target;
        }

        public WeChatNotifyEvent(WeChatNotifyEventType type) : this(type, null) { }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }
    }
}
