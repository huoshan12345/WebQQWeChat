using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace Utility.HttpAction.Action
{
    public interface IAction : IActor, IActionEventHandler
    {
    }
}
