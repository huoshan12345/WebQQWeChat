using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpActionTools.Actor
{
    /// <summary>
    /// <para>单线程的QQ内部分发器，可以同时使用多个QQ实例里</para>
    /// </summary>
    public class SimpleActorDispatcher : IActorDispatcher, IDisposable
    {
        private readonly bool _asyncExcute;
        private readonly BlockingCollection<IActor> _actorQueue;

        public SimpleActorDispatcher(bool asyncExcute = false)
        {
            _asyncExcute = asyncExcute;
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
        private bool DispatchAction(IActor actor)
        {
            if (actor == null) return true;
            if (_asyncExcute) actor.ExecuteAsync();
            else actor.Execute();
            return !(actor is ExitActor);
        }

        public void Run()
        {
            while (DispatchAction(_actorQueue.Take())) { };
        }

        public void BeginExcute()
        {
            Task.Run(() => Run());
        }

        public void Dispose()
        {
            PushActor(new ExitActor());
        }
    }
}
