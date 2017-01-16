using System.Collections.Generic;
using FclEx.Extensions;
using FclEx.Helpers;

namespace HttpAction.Event
{
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
            return target == null ? EmptyEvents[type] : new ActionEvent(type, target);
        }

        public override string ToString()
        {
            return $"{Type.GetFullDescription()}, target={Target ?? ""}]";
        }

        private static readonly IReadOnlyDictionary<ActionEventType, ActionEvent> EmptyEvents;

        static ActionEvent()
        {
            var dic = new Dictionary<ActionEventType, ActionEvent>();
            foreach (var @enum in EnumHelper.GetValues<ActionEventType>())
            {
                dic[@enum] = new ActionEvent(@enum, null);
            }
            EmptyEvents = dic;
        }

        public static ActionEvent EmptyOkEvent => EmptyEvents[ActionEventType.EvtOK];
        public static ActionEvent EmptyRepeatEvent => EmptyEvents[ActionEventType.EvtRepeat];
    }
}
