using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>基础模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public abstract class AbstractModule : IQQModule
    {
        public IQQContext Context { get; private set; }

        public void Init(IQQContext context)
        {
            Context = context;
        }
        
        public void Destroy()
        {
        }

        public IQQActionFuture PushHttpAction(IHttpAction action)
        {
            var future = new HttpActionFuture(action);	 	//替换掉原始的QQActionListener
            Context.PushActor(new HttpActor(HttpActorType.BuildRequest, Context, action));
            return future;
        }
    }
}
