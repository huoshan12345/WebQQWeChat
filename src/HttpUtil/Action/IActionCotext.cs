using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Core;

namespace HttpActionTools.Action
{
    public interface IActionCotext
    {
        IHttpService HttpService { get; }

        IActorDispatcher ActorDispatcher { get; }
    }
}
