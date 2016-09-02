using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Event.Future
{
    public abstract class AbstractActionFuture : IQQActionFuture
    {
        private volatile bool _hasEvent;
        private volatile bool _isCanceled;

        // BlockingCollection<T> is a thread-safe collection
        protected BlockingCollection<QQActionEvent> EventQueue { get;  }

        protected bool HasEvent
        {
            get { return _hasEvent; }
            private set { _hasEvent = value; } 
        }

        public bool IsCanceled
        {
            get { return _isCanceled; } 
            private set { _isCanceled = value; } 
        }

        protected CancellationTokenSource Cts
        {
            get { return _cts; }
            set { _cts = value; }
        }

        private volatile CancellationTokenSource _cts;

        public QQActionListener Listener { get; }
        public QQActionListener ProxyListener { get; }

        protected AbstractActionFuture(QQActionListener proxyListener)
        {
            _isCanceled = false;
            _hasEvent = true;
            EventQueue = new BlockingCollection<QQActionEvent>();
            ProxyListener = proxyListener;
            Listener = (sender, args) =>
            {
                ProxyListener?.Invoke(this, args);
                EventQueue.Add(args);
            };
            Cts = new CancellationTokenSource();
        }

        private QQActionEvent WaitEvent()
        {
            var Event = EventQueue.Take(Cts.Token);
            HasEvent = !IsFinalEvent(Event);
            return Event;
        }

        public QQActionEvent WaitFinalEvent()
        {
            while (!Cts.Token.IsCancellationRequested)
            {
                try
                {
                    var Event = WaitEvent();
                    if (IsFinalEvent(Event)) return Event;
                }
                catch (OperationCanceledException)
                {
                    return new QQActionEvent(QQActionEventType.EvtCanceled, this);
                }
                catch (Exception ex)
                {
                    return new QQActionEvent(QQActionEventType.EvtError, ex);
                }
            }
            return new QQActionEvent(QQActionEventType.EvtCanceled, this);
        }

        public QQActionEvent WaitFinalEvent(long timeoutMs)
        {
            _cts = new CancellationTokenSource(new TimeSpan(timeoutMs));
            return WaitFinalEvent();
        }

        public Task<QQActionEvent> WhenFinalEvent()
        {
            return Task.Run(() => WaitFinalEvent(), _cts.Token);
        }

        public Task<QQActionEvent> WhenFinalEvent(long timeoutMs)
        {
            _cts = new CancellationTokenSource(new TimeSpan(timeoutMs));
            return WhenFinalEvent();
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

        public virtual void Cancel()
        {
            if (IsCancelable() && !IsCanceled)
            {
                IsCanceled = true;
                Cts.Cancel();
                NotifyActionEvent(QQActionEventType.EvtCanceled, this);
            }
        }
    }
}
