using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpActionTools.Actor
{
    public interface IActorDispatcher
    {
        /// <summary>
        /// 把一个Actor放入线程安全的事件队列的末尾等待处理
        /// </summary>
        /// <param name="actor"></param>
        void PushActor(IActor actor);

        void BeginExcute();
    }
}
