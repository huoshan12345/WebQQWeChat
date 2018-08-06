using System;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Im.Modules.Interface;

namespace WebQQ.Im.Modules.Impl
{
    /// <summary>
    /// <para>基础模块</para>
    /// </summary>
    public abstract class QQModule : IQQModule
    {
        public IQQContext Context { get; set; }
        protected ILogger Logger => Context.Logger;
        protected QQModule(IQQContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual void Init()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
