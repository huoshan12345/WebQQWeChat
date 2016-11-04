using FxUtility.Extensions;
using System;

namespace WebWeChat.Im.Event
{    

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
