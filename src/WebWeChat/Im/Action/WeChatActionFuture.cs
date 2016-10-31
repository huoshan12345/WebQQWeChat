using Utility.HttpAction;
using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Action
{
    public class WeChatActionFuture : ActionFuture
    {
        protected IWeChatActionFactory ActionFactory { get; }
        public IWeChatContext Context { get; }

        public WeChatActionFuture(IWeChatContext context, ActionEventListener listener = null)
            : base(listener)
        {
            Context = context;
            ActionFactory = Context.GetSerivce<IWeChatActionFactory>();
        }

        public WeChatActionFuture PushAction<T>(params object[] args) where T : WeChatAction
        {
            var action = ActionFactory.CreateAction<T>(args);
            return (WeChatActionFuture)base.PushAction(action);
        }
    }
}
