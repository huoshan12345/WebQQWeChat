using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Actor
{

    /// <summary>
    /// QQ内部操作分发器接口
    /// </summary>
    public interface IQQActorDispatcher : IQQLifeCycle
    {
        /// <summary>
        /// 把一个Actor放入线程安全的事件队列的末尾等待处理
        /// </summary>
        /// <param name="actor"></param>
        void PushActor(IQQActor actor);
    }
}
