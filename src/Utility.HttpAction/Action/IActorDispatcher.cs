using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.HttpAction.Action
{
    /// <summary>
    /// 用于执行一些独立的action
    /// </summary>
    public interface IActorDispatcher : IDisposable
    {
        /// <summary>
        /// 把一个Actor放入线程安全的事件队列的末尾等待处理
        /// </summary>
        /// <param name="actor"></param>
        void PushActor(IActor actor);

        void BeginExcute();
    }
}
