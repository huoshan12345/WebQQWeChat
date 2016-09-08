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
        protected int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;
        public IActionLink ActionLink { get; set; }
        protected readonly IHttpService _httpService;

        protected AbstractHttpAction(IHttpService httpService, ActionEventListener listener)
        {
            _httpService = httpService;
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
            ExecuteAsync().Wait();
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var requestItem = BuildRequest();
                await _httpService.ExecuteHttpRequestAsync(requestItem, ActionLink?.Token ?? CancellationToken.None, this);
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
                if (ActionLink != null) ActionLink.PushAction(this);
                else Execute();
            }
            else NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public event ActionEventListener OnActionEvent;
    }
}
