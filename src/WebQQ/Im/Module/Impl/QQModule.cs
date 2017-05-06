using System;
using HttpAction.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Interface;

namespace WebQQ.Im.Module.Impl
{
    /// <summary>
    /// <para>基础模块</para>
    /// </summary>
    public abstract class QQModule : IQQModule
    {
        public IQQContext Context { get; set; }
        protected ILogger Logger => Context.GetSerivce<ILogger>();
        protected IConfigurationRoot Config => Context.GetSerivce<IConfigurationRoot>();
        protected SessionModule Session => Context.GetModule<SessionModule>();
        protected StoreModule Store => Context.GetModule<StoreModule>();

        protected QQModule(IQQContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
