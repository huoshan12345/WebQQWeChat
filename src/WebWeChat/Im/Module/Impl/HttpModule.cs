using System;
using HttpActionFrame.Core;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class HttpModule : HttpService, IHttpModule
    {
        public void Init(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        public void Destroy()
        {
            Dispose();
        }

        public IWeChatContext Context { get; private set; }
    }
}
