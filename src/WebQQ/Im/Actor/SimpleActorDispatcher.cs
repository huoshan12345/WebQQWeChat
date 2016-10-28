using HttpActionFrame.Actor;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actor
{
    /// <summary>
    /// <para>单线程的内部分发器，可以同时使用多个QQ实例里</para>
    /// </summary>
    public class QQActorDispatcher : SimpleActorDispatcher, IQQActorDispatcher
    {
        public void Init(IQQContext context)
        {
            BeginExcute();
        }

        public void Destroy()
        {
            Dispose();
        }
    }
}
