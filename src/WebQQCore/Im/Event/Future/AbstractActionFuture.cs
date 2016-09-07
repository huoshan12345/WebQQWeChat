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
        private readonly object _syncObj = new object();

        private volatile QQActionEvent _finalEvent;

        // BlockingCollection<T> is a thread-safe collection
        protected BlockingCollection<QQActionEvent> EventQueue { get; }

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
            if (EventQueue.IsAddingCompleted) return FinalEvent;
            var Event = EventQueue.Take(_cts.Token);
            HasEvent = !IsFinalEvent(Event);
            return Event;
        }

        public QQActionEvent WaitFinalEvent(CancellationToken token)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
            return WaitFinalEvent();
        }

        public QQActionEvent WaitFinalEvent()
        {
            if (EventQueue.IsAddingCompleted) return FinalEvent;
            while (!Token.IsCancellationRequested)
            {
                try
                {
                    var Event = WaitEvent();
                    if (IsFinalEvent(Event))
                    {
                        EventQueue.CompleteAdding();
                        FinalEvent = Event;
                        return FinalEvent;
                    }
                }
                catch (OperationCanceledException)
                {
                    FinalEvent = new QQActionEvent(QQActionEventType.EvtCanceled, this);
                    return FinalEvent;
                }
                catch (Exception ex)
                {
                    FinalEvent = new QQActionEvent(QQActionEventType.EvtError, ex);
                    return FinalEvent;

                }
            }
            FinalEvent = new QQActionEvent(QQActionEventType.EvtCanceled, this);
            return FinalEvent;
        }

        public QQActionEvent WaitFinalEvent(int millisecond)
        {
            var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, millisecond));
            return WaitFinalEvent(cts.Token);
        }

        public Task<QQActionEvent> WhenFinalEvent()
        {
            return Task.Run(() => WaitFinalEvent(), Token);
        }

        public Task<QQActionEvent> WhenFinalEvent(CancellationToken token)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
            return WhenFinalEvent();
        }

        public CancellationToken Token => Cts.Token;

        protected QQActionEvent FinalEvent
        {
            get { return _finalEvent; }
            set { _finalEvent = value; }
        }

        public Task<QQActionEvent> WhenFinalEvent(int millisecond)
        {
            var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, millisecond));
            return WhenFinalEvent(cts.Token);
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
            }

        }
    }
}
