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
        private readonly IHttpActionCotext _actionCotext;
        private readonly IActionLink _actionLink;
        private int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;

        protected AbstractHttpAction(IActionLink actionLink, IHttpActionCotext actionCotext, ActionEventListener listener)
        {
            _actionLink = actionLink;
            _actionCotext = actionCotext;
            _listener = listener;
            OnActionEvent += listener;
        }

        public abstract HttpRequestItem BuildRequest();

        public void NotifyActionEvent(ActionEvent actionEvent)
        {
            switch (actionEvent.Type)
            {
                case ActionEventType.EvtOK:
                    break;
                case ActionEventType.EvtError:
                    break;
                case ActionEventType.EvtCanceled:
                _actionLink.Terminate(this, actionEvent);
                    break;
                case ActionEventType.EvtRetry:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            OnActionEvent?.Invoke(this, actionEvent);
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
                var responseItem = await _actionCotext.HttpService.ExecuteHttpRequestAsync(requestItem, _actionLink.Token, this);
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

        public void Begin()
        {
            _actionCotext.ActorDispatcher.PushActor(this);
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
                NotifyActionEvent(new ActionEvent(ActionEventType.EvtRetry, ex));
                _actionCotext.ActorDispatcher.PushActor(this);
            }
            else NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public event ActionEventListener OnActionEvent;
    }
}
