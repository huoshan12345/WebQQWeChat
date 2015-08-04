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
    public class AbstractModule : IQQModule
    {
        public QQContext Context { get; private set; }

        public void Init(QQContext context)
        {
            Context = context;
        }
        
        public void Destroy()
        {
        }

        public QQActionFuture PushHttpAction(IHttpAction action)
        {
            QQActionFuture future = new HttpActionFuture(action);	 	//替换掉原始的QQActionListener
            this.Context.PushActor(new HttpActor(HttpActorType.BUILD_REQUEST, this.Context, action));
            return future;
        }

    }
}
