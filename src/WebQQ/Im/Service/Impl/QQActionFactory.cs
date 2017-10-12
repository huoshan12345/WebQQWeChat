using WebQQ.Im.Core;
using WebQQ.Im.Service.Interface;
using System.Reflection;
using HttpAction.Actions;
using WebQQ.Im.Actions;

namespace WebQQ.Im.Service.Impl
{
    public class QQActionFactory : ActionFactory, IQQActionFactory
    {
        public QQActionFactory(IQQContext context)
        {
            Context = context;
        }

        public IQQContext Context { get;}
        

        public override IAction CreateAction<T>(params object[] parameters)
        {
            var type = typeof(T);
            // 把Context作为第一个参数加进去
            if (typeof(WebQQAction).GetTypeInfo().IsAssignableFrom(type))
            {
                var newArgs = new object[parameters.Length + 1];
                newArgs[0] = Context;
                parameters.CopyTo(newArgs, 1);
                parameters = newArgs;
            }
            return base.CreateAction<T>(parameters);
        }

        public void Dispose() { }
    }
}
