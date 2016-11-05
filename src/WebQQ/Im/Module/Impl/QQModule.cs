using System;
using HttpAction.Action;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Interface;

namespace WebQQ.Im.Module.Impl
{
    /// <summary>
    /// <para>基础模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public abstract class QQModule : IQQModule
    {
        protected IQQContext Context { get; private set; }

        public IActorDispatcher Dispatcher { get; }

        protected QQModule(IQQContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            Dispatcher = Context.GetSerivce<IActorDispatcher>();
        }
    }
}
