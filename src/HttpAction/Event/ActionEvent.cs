using System.Collections.Generic;
using FclEx.Extensions;
using FclEx.Helpers;

namespace HttpAction.Event
{
    /// <summary>
    /// 用来表示一个action的执行结果
    /// 由于有地方需要引用传递，所以不可改为值类型。
    /// </summary>
    public class ActionEvent
    {
        public ActionEventType Type { get; set; }
        public object Target { get; }

        private ActionEvent(ActionEventType type, object target)
        {
            Type = type;
            Target = target;
        }

        public static ActionEvent CreateEvent(ActionEventType type, object target)
        {
            return target == null ? _emptyEvents[type] : new ActionEvent(type, target);
        }

        public static ActionEvent CreateOkEvent(object target)
        {
            return CreateEvent(ActionEventType.EvtOK, target);
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }

        private static readonly IReadOnlyDictionary<ActionEventType, ActionEvent> _emptyEvents;

        static ActionEvent()
        {
            var dic = new Dictionary<ActionEventType, ActionEvent>();
            foreach (var @enum in EnumHelper.GetValues<ActionEventType>())
            {
                dic[@enum] = new ActionEvent(@enum, null);
            }
            _emptyEvents = dic;
        }

        public static ActionEvent EmptyOkEvent => _emptyEvents[ActionEventType.EvtOK];
        public static ActionEvent EmptyRepeatEvent => _emptyEvents[ActionEventType.EvtRepeat];
    }
}
