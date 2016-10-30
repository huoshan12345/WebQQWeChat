using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class WebWeChatActionFuture : ActionFuture
    {
        protected IWeChatContext Context { get; }

        public WebWeChatActionFuture(IWeChatContext context, ActionEventListener listener = null)
            : base(listener)
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
