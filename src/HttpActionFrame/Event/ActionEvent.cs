using System.ComponentModel;
using Utility.Extensions;

namespace HttpActionFrame.Event
{
    [Description("动作事件类型")]
    public enum ActionEventType
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

    public class ActionEvent
    {
        public ActionEventType Type { get; }
        public object Target { get; }

        public ActionEvent(ActionEventType type, object target)
        {
            Type = type;
            Target = target;
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }

        public static ActionEvent EmptyOkEvent { get; } = new ActionEvent(ActionEventType.EvtOK, null);
    }
}
