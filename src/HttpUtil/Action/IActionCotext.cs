using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Core;
using Microsoft.Extensions.Logging;

namespace HttpActionTools.Action
{
    public interface IActionCotext
    {
        IActorDispatcher ActorDispatcher { get; }

        ILogger Logger { get; }
    }
}
