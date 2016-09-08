using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public interface IAction : IActor, IActionEventHandler
    {
    }
}
