using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction.Action;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Service.Interface
{
    public interface IWeChatActionFuture
    {
        IWeChatContext Context { get; }

        IWeChatActionFuture PushAction<T>(params object[] args) where T : WeChatAction;
    }
}
