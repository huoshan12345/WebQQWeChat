using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Core;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public abstract class AbstractHttpAction : IHttpAction
    {
        private readonly ActionEventListener _listener;
        private readonly IActorDispatcher _dispatcher;
        private readonly object _syncObj = new object();
        private bool _isWaiting;
        private ActionEvent _finalEvent;
        protected CancellationTokenSource _cts;

        protected AbstractHttpAction(IActorDispatcher dispatcher, ActionEventListener listener)
        {
            _dispatcher = dispatcher;
            _listener = listener;
        }

        protected BlockingCollection<ActionEvent> EventQueue { get; private set; } = new BlockingCollection<ActionEvent>();

        public abstract HttpRequestItem BuildRequest();

        public void NotifyActionEvent(ActionEvent actionEvent)
        {
            EventQueue.Add(actionEvent);
            _listener?.Invoke(actionEvent);
        }

        public IHttpService HttpService { get; }

        public void Cancel()
        {
            _cts.Cancel();
            IsCanceled = true;
        }

        private ActionEvent WaitEvent(CancellationToken token)
        {
            if (EventQueue.IsAddingCompleted) return _finalEvent;
            var Event = EventQueue.Take(token);
            return Event;
        }

        public IAction Begin()
        {
            EventQueue = new BlockingCollection<ActionEvent>();
            _cts = new CancellationTokenSource();
            return this;
        }

        public bool IsCanceled { get; private set; }

        public ActionEvent WaitFinalEvent()
        {
            return WaitFinalEvent(CancellationToken.None);
        }

        public ActionEvent WaitFinalEvent(int second)
        {
            var token = new CancellationTokenSource(second * 1000).Token;
            return WaitFinalEvent(token);
        }

        public ActionEvent WaitFinalEvent(CancellationToken token)
        {
            // double-check
            if (_isWaiting) throw new Exception("The action is already waiting...");
            lock (_syncObj)
            {
                if (_isWaiting) throw new Exception("The action is already waiting...");
                _isWaiting = true;
            }
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
            // 保证以下代码是单线程执行
            if (!EventQueue.IsAddingCompleted)
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var Event = WaitEvent(token);
                        if (IsFinalEvent(Event))
                        {
                            EventQueue.CompleteAdding();
                            _finalEvent = Event;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _finalEvent = new ActionEvent(ActionEventType.EvtCanceled, this);
                    }
                    catch (Exception ex)
                    {
                        _finalEvent = new ActionEvent(ActionEventType.EvtError, ex);
                    }
                }
                _finalEvent = new ActionEvent(ActionEventType.EvtCanceled, this);
            }
            _isWaiting = false;
            return _finalEvent;
        }

        public Task<ActionEvent> WaitFinalEventAsync()
        {
            return Task.Run(() => WaitFinalEvent(), CancellationToken.None);
        }

        public Task<ActionEvent> WaitFinalEventAsync(int second)
        {
            var token = new CancellationTokenSource(second * 1000).Token;
            return WaitFinalEventAsync(token);
        }

        public Task<ActionEvent> WaitFinalEventAsync(CancellationToken token)
        {
            return Task.Run(() => WaitFinalEvent(token), token);
        }

        public CancellationToken Token => _cts.Token;

        protected virtual bool IsFinalEvent(ActionEvent Event)
        {
            var type = Event.Type;
            return type == ActionEventType.EvtCanceled
                    || type == ActionEventType.EvtError
                    || type == ActionEventType.EvtOK;
        }

        public void OnHttpHeader(HttpResponseItem responseItem)
        {
            throw new NotImplementedException();
        }

        public void OnHttpContent(HttpResponseItem responseItem)
        {
            throw new NotImplementedException();
        }

        public void OnHttpRead(ProgressEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnHttpWrite(ProgressEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnHttpError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
