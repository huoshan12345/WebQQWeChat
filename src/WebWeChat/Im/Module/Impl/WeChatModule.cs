using System;
using HttpActionFrame.Actor;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public abstract class WeChatModule : IWeChatModule
    {
        protected IActorDispatcher ActorDispatcher { get; private set; }

        public virtual void Init(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            ActorDispatcher = context.GetSerivce<IActorDispatcher>();
        }

        public virtual void Destroy()
        {
        }

        public IWeChatContext Context { get; private set; }
    }
}
