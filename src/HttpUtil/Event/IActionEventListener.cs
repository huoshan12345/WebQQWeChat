using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Action;

namespace HttpActionTools.Event
{
    public delegate void ActionEventListener(IAction sender, ActionEvent actionEvent);

    public interface IActionEventHandler
    {
        event ActionEventListener OnActionEvent;
    }
}
