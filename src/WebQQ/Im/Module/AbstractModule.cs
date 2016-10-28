using WebQQ.Im.Actor;
using WebQQ.Im.Core;

namespace WebQQ.Im.Module
{
    /// <summary>
    /// <para>基础模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public abstract class AbstractModule : IQQModule
    {
        protected IQQContext Context { get; private set; }

        protected IQQActorDispatcher ActorDispatcher { get; private set; }

        public virtual void Init(IQQContext context)
        {
            Context = context;
            ActorDispatcher = context.GetSerivce<IQQActorDispatcher>(QQServiceType.Actor);
        }

        public virtual void Destroy()
        {
        }
    }
}
