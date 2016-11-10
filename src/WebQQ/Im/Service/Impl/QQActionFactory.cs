using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Action;
using WebQQ.Im.Action;
using WebQQ.Im.Core;
using WebQQ.Im.Service.Interface;
using System.Reflection;

namespace WebQQ.Im.Service.Impl
{
    public class QQActionFactory : ActionFactory, IQQActionFactory, IQQService
    {
        public IQQContext Context { get; }

        public QQActionFactory(IQQContext context)
        {
            Context = context;
        }

        public override IAction CreateAction<T>(params object[] parameters)
        {
            var type = typeof(T);
            // 把Context作为第一个参数加进去
            if (typeof(QQAction).GetTypeInfo().IsAssignableFrom(type))
            {
                var newArgs = new object[parameters.Length + 1];
                newArgs[0] = Context;
                parameters.CopyTo(newArgs, 1);
                parameters = newArgs;
            }
            return base.CreateAction<T>(parameters);
        }
    }
}
