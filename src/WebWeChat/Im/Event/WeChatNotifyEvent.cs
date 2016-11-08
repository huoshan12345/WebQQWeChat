using FxUtility.Extensions;
using System;
using System.Collections.Concurrent;

namespace WebWeChat.Im.Event
{    

    public class WeChatNotifyEvent : EventArgs
    {
        private static readonly ConcurrentDictionary<WeChatNotifyEventType, WeChatNotifyEvent> EmptyEvents
            = new ConcurrentDictionary<WeChatNotifyEventType, WeChatNotifyEvent>();

        public WeChatNotifyEventType Type { get; }
        public object Target { get; }

        private WeChatNotifyEvent(WeChatNotifyEventType type, object target = null)
        {
            Type = type;
            Target = target;
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }

        public static WeChatNotifyEvent CreateEvent(WeChatNotifyEventType type, object target = null)
        {
            return target == null ? EmptyEvents.GetOrAdd(type, key => new WeChatNotifyEvent(key)) : new WeChatNotifyEvent(type, target);
        }
    }
}
