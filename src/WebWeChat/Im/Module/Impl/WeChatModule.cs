using System;
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
        }

        public IWeChatContext Context { get; }
    }
}
