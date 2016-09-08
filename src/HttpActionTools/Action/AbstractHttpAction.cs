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
        // private readonly ActionEventListener _listener;
        protected readonly IHttpActionLink _actionLink;
        protected int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;

        protected AbstractHttpAction(IHttpActionLink actionLink, ActionEventListener listener)
        {
            _actionLink = actionLink;
            // _listener = listener;
            OnActionEvent += listener;
        }

        public abstract HttpRequestItem BuildRequest();

        public virtual void NotifyActionEvent(ActionEvent actionEvent)
        {
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
                var responseItem = await _actionLink.HttpService.ExecuteHttpRequestAsync(requestItem, _actionLink.Token, this);
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
                _actionLink.PushAction(this);
            }
            else NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public event ActionEventListener OnActionEvent;
    }
}
