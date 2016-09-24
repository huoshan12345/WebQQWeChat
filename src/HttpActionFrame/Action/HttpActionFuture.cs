using HttpActionFrame.Actor;
using HttpActionFrame.Core;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public class HttpActionFuture: ActionFuture, IHttpActionFuture
    {
        public HttpActionFuture(IActorDispatcher actorDispatcher, IHttpService httpService, ActionEventListener listener)
            : base(actorDispatcher, listener)
        {
            HttpService = httpService;
        }

        public IHttpService HttpService { get; }
    }
}
