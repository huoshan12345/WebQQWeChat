using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    /// <summary>
    /// <para>多线程的QQ内部分发器</para>
    /// </summary>
    public class MutipleActorDispatcher : IQQActorDispatcher
    {
        private volatile CancellationTokenSource _cts;
        private readonly ConcurrentDictionary<int, Tuple<BlockingCollection<IQQActor>, Task>> _actorQueues;
        private readonly Random _random;
        private readonly int _pollThreadNum;

        public MutipleActorDispatcher(int pollThreadNum)
        {
            if(pollThreadNum <= 0) throw new ArgumentOutOfRangeException(nameof(pollThreadNum));

            _pollThreadNum = pollThreadNum;
            _cts = new CancellationTokenSource();
            _actorQueues = new ConcurrentDictionary<int, Tuple<BlockingCollection<IQQActor>, Task>>();
            _random = new Random();
        }

        private void PushActor(IQQActor actor, int actorQueuesIndex)
        {
            var actorQueue = _actorQueues.GetOrAdd(actorQueuesIndex, (i) =>
            {
                var col = new BlockingCollection<IQQActor>();
                var task = Task.Run(() => Run(col), _cts.Token);
                return new Tuple<BlockingCollection<IQQActor>, Task>(col, task);
            });
            actorQueue.Item1.Add(actor);
        }

        public void PushActor(IQQActor actor)
        {
            switch (actor.Type)
            {
                case QQActorType.PollMsgActor:
                {
                    var index = _random.Next(0, _pollThreadNum);
                    PushActor(actor, index);
                    break;
                }

                default:
                {
                    PushActor(actor, -1);
                    break;
                }
            }
        }


        private void Run(BlockingCollection<IQQActor> actorQueue)
        {
            try
            {
                while (!_cts.IsCancellationRequested
                       && DispatchAction(actorQueue.Take(_cts.Token)))
                {
                }
            }
            catch (OperationCanceledException)
            {
                // 发生取消异常是正常的
            }

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

        public void Init(IQQContext context)
        {
            for (var i = -1; i < _pollThreadNum; ++i)
            {
                var col = new BlockingCollection<IQQActor>();
                var task = Task.Run(() => Run(col), _cts.Token);
                _actorQueues[i] = new Tuple<BlockingCollection<IQQActor>, Task>(col, task);
            }
        }

        public void Destroy()
        {
            //var exitActor = new ExitActor();
            //foreach (var queue in _actorQueues)
            //{
            //    queue.Value.Item1.Add(exitActor);
            //}
            // PushActor(new ExitActor());
            _cts.Cancel();
        }
    }
}
