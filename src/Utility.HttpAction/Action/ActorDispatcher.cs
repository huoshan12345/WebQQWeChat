using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace Utility.HttpAction.Action
{
    public class ActorDispatcher : IActorDispatcher
    {
        private readonly BlockingCollection<IActor> _actorQueue;

        public ActorDispatcher()
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
            actor.ExecuteAsync().ContinueWith(result =>
            {
                if(result.Status != TaskStatus.RanToCompletion) return;
                var type = result.Result.Type;
                if (type == ActionEventType.EvtRepeat || type == ActionEventType.EvtRetry)
                {
                    DispatchAction(actor);
                }
            } );
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
