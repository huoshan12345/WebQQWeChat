using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    /// <summary>
    /// <para>单线程的QQ内部分发器，可以同时使用多个QQ实例里</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class ThreadActorDispatcher : IQQActorDispatcher
    {
        private readonly BlockingCollection<IQQActor> _actorQueue = null;
        private readonly BlockingCollection<IQQActor> _pollActorQueue = null;
        //private BlockingCollection<QQActor> robotReplyActorQueue = null;
        private Task _dispatchThread = null;
        private Task _pollDispatchThread = null;
        //private Task robotReplyDispatchThread = null;

        public ThreadActorDispatcher()
        {
            _actorQueue = new BlockingCollection<IQQActor>();
            _pollActorQueue = new BlockingCollection<IQQActor>();
            // robotReplyActorQueue = new BlockingCollection<QQActor>();
        }

        public void PushActor(IQQActor actor)
        {
            try
            {
                switch (actor.Type)
                {
                    case QQActorType.PollMsgActor:
                    _pollActorQueue.Add(actor);
                    break;

                    case QQActorType.GetRobotReply:
                    _actorQueue.Add(actor);
                    break;

                    default:
                    _actorQueue.Add(actor);
                    break;
                }
            }
            catch { }
        }

        /// <summary>
        /// 执行一个QQActor，返回是否继续下一个actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        private bool DispatchAction(IQQActor actor)
        {
            if (actor == null) return true;
            actor.Execute();
            return !(actor is ExitActor);
        }

        public void Run()
        {
            while (DispatchAction(_actorQueue.Take())) { };
        }

        public void DoPoll()
        {
            while (DispatchAction(_pollActorQueue.Take())) { };
        }

        public void Init(IQQContext context)
        {
            if (_dispatchThread == null)
            {
                _dispatchThread = Task.Run(() => Run());
            }
            if (_pollDispatchThread == null)
            {
                _pollDispatchThread = Task.Run(() => DoPoll());
            }
        }

        public void Destroy()
        {
            try
            {
                PushActor(new ExitActor());
                // _dispatchThread.Dispose();
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
            }
        }

        /// <summary>
        /// 一个伪Actor只是为了让ActorLoop停下来
        /// </summary>
        public class ExitActor : IQQActor
        {
            public void Execute()
            {
                //do nothing
            }

            public QQActorType Type => QQActorType.SimpleActor;

            public Task ExecuteAsync()
            {
                return Task.Run(() => Execute());
            }
        }
    }
}
