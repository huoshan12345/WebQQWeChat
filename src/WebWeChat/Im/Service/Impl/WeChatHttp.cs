using System;
using Utility.HttpAction.Service;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Service.Impl
{
    public class WeChatHttp : HttpService, IWeChatHttp
    {
        public IWeChatContext Context { get; }

        public WeChatHttp(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }
    }
}
