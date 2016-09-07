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
        private readonly IActionCotext _actionCotext;
        private readonly object _syncObj = new object();
        private bool _isWaiting;
        private ActionEvent _finalEvent;
        protected CancellationTokenSource _cts;
        private int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;
        protected ManualResetEvent _waitHandle;

        protected AbstractHttpAction(IActionCotext actionCotext, ActionEventListener listener)
        {
            _actionCotext = actionCotext;
            _listener = listener;
        }

        public abstract HttpRequestItem BuildRequest();

        public void NotifyActionEvent(ActionEvent actionEvent)
        {
            if (IsFinalEvent(actionEvent))
            {
                _finalEvent = actionEvent;
                _waitHandle.Set();
            }
            _listener?.Invoke(actionEvent);
        }

        public void Cancel()
        {
            _cts.Cancel();
            IsCanceled = true;
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtCanceled, this));
        }

        public void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var requestItem = BuildRequest();
                var responseItem = await _actionCotext.HttpService.ExecuteHttpRequestAsync(requestItem, Token, this);
                if (responseItem.Success)
                {
                    NotifyActionEvent(new ActionEvent(ActionEventType.EvtOK, responseItem));
                }
            }
            catch (TaskCanceledException)
            {
                NotifyActionEvent(new ActionEvent(ActionEventType.EvtCanceled, this));
            }
            catch (OperationCanceledException)
            {
                NotifyActionEvent(new ActionEvent(ActionEventType.EvtCanceled, this));
            }
            catch (Exception ex)
            {
                OnHttpError(ex);
            }
        }

        public IAction Begin()
        {
            _waitHandle = new ManualResetEvent(false);
            _cts = new CancellationTokenSource();
            _actionCotext.ActorDispatcher.PushActor(this);
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
            if (_isWaiting) throw new Exception("this action is still waiting...");
            lock (_syncObj)
            {
                if (_isWaiting) throw new Exception("this action is still waiting...");
                _waitHandle.WaitOne();
                return _finalEvent;
            }
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

        public virtual void OnHttpHeader(HttpResponseItem responseItem)
        {
        }

        public virtual void OnHttpContent(HttpResponseItem responseItem)
        {
        }

        public virtual void OnHttpRead(ProgressEventArgs args)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtRead, args));
        }

        public virtual void OnHttpWrite(ProgressEventArgs args)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtWrite, args));
        }

        public virtual void OnHttpError(Exception ex)
        {
            if (++_retryTimes < MaxReTryTimes)
            {
                _actionCotext.ActorDispatcher.PushActor(this);
            }
            else NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }
    }
}
