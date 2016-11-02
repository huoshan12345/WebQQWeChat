using System;
using System.Reflection;
using Utility.HttpAction.Action;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Service.Impl
{
    public class WeChatActionFactory : ActionFactory, IWeChatActionFactory
    {
        public IWeChatContext Context { get; }

        
        public WeChatActionFactory(IWeChatContext context)
        {
            Context = context;
        }

        public override IAction CreateAction(Type actionType, params object[] args)
        {
            if (typeof(WeChatAction).GetTypeInfo().IsAssignableFrom(actionType))
            {
                var newArgs = new object[args.Length + 1];
                newArgs[0] = Context;
                args.CopyTo(newArgs, 1);
                args = newArgs;
            }
            return base.CreateAction(actionType, args);
        }
    }
}
