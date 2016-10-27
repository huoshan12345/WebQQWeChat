using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace HttpActionFrame.Actor
{
    /// <summary>
    /// <para>单线程的QQ内部分发器，可以同时使用多个QQ实例里</para>
    /// </summary>
    public class SimpleActorDispatcher : IActorDispatcher
    {
        private readonly BlockingCollection<IActor> _actorQueue;

        public SimpleActorDispatcher()
        {
            _actorQueue = new BlockingCollection<IActor>();
        }

        public void PushActor(IActor actor)
        {
            _actorQueue.Add(actor);
        }

        /// <summary>
        /// 执行一个QQActor，返回是否继续下一个actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        private static bool DispatchAction(IActor actor)
        {
            if (actor == null) return true;
            actor.ExecuteAsync();
            return !(actor is ExitActor);
        }

        public virtual void BeginExcute()
        {
            Task.Run(() =>
            {
                while (DispatchAction(_actorQueue.Take())) { }
                _actorQueue.Dispose();
            });
        }

        public void Dispose()
        {
            PushActor(new ExitActor());
        }
    }
}
