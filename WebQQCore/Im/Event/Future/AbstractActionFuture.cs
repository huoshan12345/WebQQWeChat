using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Event.Future
{
    public abstract class AbstractActionFuture : IQQActionFuture
    {
        private readonly BlockingCollection<QQActionEvent> _eventQueue;
        private bool _hasEvent;
        public QQActionListener Listener { get; set; }
        public QQActionListener ProxyListener { get; set; }

        protected AbstractActionFuture(QQActionListener proxyListener)
        {
            _hasEvent = true;
            _eventQueue = new BlockingCollection<QQActionEvent>();
            ProxyListener = proxyListener;
            Listener = (sender, args) =>
            {
                ProxyListener?.Invoke(sender, args);
                _eventQueue.Add(args);
            };
        }

        public bool IsCanceled { get; set; }

        public QQActionEvent WaitEvent()
        {
            if (!_hasEvent)
            {
                return null;
            }
            try
            {
                var Event = _eventQueue.Take();
                _hasEvent = !IsFinalEvent(Event);
                return Event;
            }
            catch (ThreadInterruptedException e)
            {
                throw new QQException(QQErrorCode.WAIT_INTERUPPTED, e);
            }
        }

        public QQActionEvent WaitEvent(long timeoutMs)
        {
            QQActionEvent Event = null;
            if (!_hasEvent)
            {
                return null;
            }
            try
            {
                _eventQueue.TryTake(out Event, (int)timeoutMs);
            }
            catch (Exception e)
            {
                // throw new QQException(QQErrorCode.WAIT_INTERUPPTED, e);
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.WAIT_INTERUPPTED, e));
            }

            if (Event == null)
            {
                // throw new QQException(QQErrorCode.WAIT_TIMEOUT);
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.WAIT_TIMEOUT));
            }
            else
            {
                _hasEvent = !IsFinalEvent(Event);
            }
            return Event;
        }

        public QQActionEvent WaitFinalEvent()
        {
            QQActionEvent Event = null;
            while ((Event = WaitEvent()) != null)
            {
                if (IsFinalEvent(Event))
                {
                    return Event;
                }
            }
            return new QQActionEvent(QQActionEventType.EvtError, this);
        }

        public QQActionEvent WaitFinalEvent(long timeoutMs)
        {
            QQActionEvent Event = null;
            var start = DateTime.Now.CurrentTimeMillis();
            while ((Event = WaitEvent(timeoutMs)) != null)
            {
                var end = DateTime.Now.CurrentTimeMillis();
                if (IsFinalEvent(Event))
                {
                    return Event;
                }
                else
                {
                    timeoutMs -= end - start;
                    start = DateTime.Now.CurrentTimeMillis();
                }
            }
            return new QQActionEvent(QQActionEventType.EvtError, this);
        }

        public Task<QQActionEvent> WhenEvent()
        {
            return Task.Run(() =>
            {
                try
                {
                    var Event = _eventQueue.Take();
                    _hasEvent = !IsFinalEvent(Event);
                    return Event;
                }
                catch (OperationCanceledException ex)
                {
                    throw new QQException(QQErrorCode.CANCELED, ex);
                }
                catch (Exception ex)
                {
                    throw new QQException(QQErrorCode.UNKNOWN_ERROR, ex);
                }
            });
        }

        public Task<QQActionEvent> WhenEvent(CancellationToken token)
        {
            return Task.Run(() =>
            {
                try
                {
                    var Event = _eventQueue.Take(token);
                    _hasEvent = !IsFinalEvent(Event);
                    return Event;
                }
                catch (OperationCanceledException ex)
                {
                    throw new QQException(QQErrorCode.CANCELED, ex);
                }
                catch (Exception ex)
                {
                    throw new QQException(QQErrorCode.UNKNOWN_ERROR, ex);
                }
            }, token);

        }

        public Task<QQActionEvent> WhenFinalEvent()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    var Event = WhenEvent().Result;
                    if (Event == null) break;
                    if (IsFinalEvent(Event)) return Event;
                }
                return new QQActionEvent(QQActionEventType.EvtError, this);
            });
        }

        public Task<QQActionEvent> WhenFinalEvent(CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    var Event = WhenEvent(token).Result;
                    if (Event == null) break;
                    if (IsFinalEvent(Event)) return Event;
                }
                return new QQActionEvent(QQActionEventType.EvtError, this);
            }, token);
        }

        public Task<QQActionEvent> WhenFinalEvent(int timeoutMs)
        {
            return WhenFinalEvent(new CancellationTokenSource(timeoutMs).Token);
        }

        private bool IsFinalEvent(QQActionEvent Event)
        {
            var type = Event.Type;
            return type == QQActionEventType.EvtCanceled
                    || type == QQActionEventType.EvtError
                    || type == QQActionEventType.EvtOK;
        }

        public void NotifyActionEvent(QQActionEventType type, object target)
        {
            Listener?.Invoke(this, new QQActionEvent(type, target));
        }

        public abstract bool IsCancelable();

        public abstract void Cancel();
    }
}
