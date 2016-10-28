using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class WebWeChatActionFuture : ActionFuture
    {
        protected IWeChatContext Context { get; }

        public WebWeChatActionFuture(IWeChatContext context, ActionEventListener listener = null)
            : base(context.GetSerivce<IActorDispatcher>(), listener)
        {
            Context = context;
        }

        public override IActionFuture PushAction(IAction action)
        {
            (action as WebWeChatAction)?.SetContext(Context);
            return base.PushAction(action);
        }
    }
}
