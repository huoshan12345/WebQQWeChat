using System;
using Utility.HttpAction.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public abstract class WeChatModule : IWeChatModule
    {
        protected WeChatModule(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            Dispatcher = Context.GetSerivce<IActorDispatcher>();
        }

        public IWeChatContext Context { get; }

        public IActorDispatcher Dispatcher { get; }
    }
}
