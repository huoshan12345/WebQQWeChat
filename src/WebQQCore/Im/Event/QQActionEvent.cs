using System;
using System.ComponentModel;
using System.Reflection;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Im.Event
{
    [Description("QQ动作事件类型")]
    public enum QQActionEventType
    {
        [Description("成功")]
        EvtOK,
        [Description("错误")]
        EvtError,
        [Description("写入")]
        EvtWrite,
        [Description("读取")]
        EvtRead,
        [Description("取消")]
        EvtCanceled,
        [Description("重试")]
        EvtRetry,
    }

    public class QQActionEvent : QQEvent
    {
        public QQActionEventType Type { get; }
        public object Target { get; }

        public QQActionEvent(QQActionEventType type, object target)
        {
            Type = type;
            Target = target;
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }
    }
}
