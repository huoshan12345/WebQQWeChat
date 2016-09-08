using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Core;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public class HttpActionLink: ActionLink, IHttpActionLink
    {
        public HttpActionLink(IActorDispatcher actorDispatcher, IHttpService httpService, ActionEventListener listener)
            : base(actorDispatcher, listener)
        {
            HttpService = httpService;
        }

        public IHttpService HttpService { get; }
    }
}
