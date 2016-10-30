using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Extensions;

namespace Utility.HttpAction.Event
{
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
