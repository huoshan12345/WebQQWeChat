using System;
using Utility.HttpAction.Service;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class HttpModule : HttpService, IHttpModule
    {
        public IWeChatContext Context { get; }

        public HttpModule(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }
    }
}
