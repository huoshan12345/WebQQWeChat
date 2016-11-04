using System.Collections.Concurrent;
using System.Threading.Tasks;
using TaskExtensions = FxUtility.Extensions.TaskExtensions;

namespace HttpAction.Action
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
        /// 执行一个Actor，返回是否继续下一个actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        private static bool DispatchAction(IActor actor)
        {
            if (actor == null) return true;
            TaskExtensions.Forget(actor.ExecuteAsyncAuto());
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
