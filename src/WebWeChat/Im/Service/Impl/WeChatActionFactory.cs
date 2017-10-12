using System.Reflection;
using HttpAction.Actions;
using WebWeChat.Im.Actions;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Service.Impl
{
    public class WeChatActionFactory : ActionFactory, IWeChatActionFactory
    {
        public IWeChatContext Context { get;}

        public WeChatActionFactory(IWeChatContext context)
        {
            Context = context;
        }

        public override IAction CreateAction<T>(params object[] parameters)
        {
            var type = typeof(T);
            if (typeof(WebWeChatAction).GetTypeInfo().IsAssignableFrom(type))
            {
                var newArgs = new object[parameters.Length + 1];
                newArgs[0] = Context;
                parameters.CopyTo(newArgs, 1);
                parameters = newArgs;
            }
            return base.CreateAction<T>(parameters);
        }

        public void Dispose()
        {
        }
    }
}
