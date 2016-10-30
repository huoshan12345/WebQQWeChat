using System;
using System.Threading;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public abstract class AbstractHttpAction : IHttpAction
    {
        // private readonly ActionEventListener _listener;
        protected int _excuteTimes;
        protected int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;
        public IActionFuture ActionFuture { get; set; }
        protected IHttpService HttpService { get; set; }

        protected AbstractHttpAction(IHttpService httpHttpService, ActionEventListener listener = null)
        {
            HttpService = httpHttpService;
            // _listener = listener;
            OnActionEvent += listener;
        }

        public abstract HttpRequestItem BuildRequest();

        public virtual void NotifyActionEvent(ActionEvent actionEvent)
        {
            OnActionEvent?.Invoke(this, actionEvent);
        }

        public virtual void NotifyActionEvent(ActionEventType type, object target = null)
        {
            NotifyActionEvent(new ActionEvent(type, target));
        }

        public virtual async Task ExecuteAsync()
        {
            _excuteTimes++;
            try
            {
                var requestItem = BuildRequest();
                var token = ActionFuture?.Token ?? CancellationToken.None;
                await HttpService.ExecuteHttpRequestAsync(requestItem, token, this);
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
            NotifyActionEvent(ActionEventType.EvtOK, responseItem);
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
            NotifyActionEvent(++_retryTimes < MaxReTryTimes
                ? new ActionEvent(ActionEventType.EvtRetry, ex)
                : new ActionEvent(ActionEventType.EvtError, ex));
        }

        public event ActionEventListener OnActionEvent;
    }
}
