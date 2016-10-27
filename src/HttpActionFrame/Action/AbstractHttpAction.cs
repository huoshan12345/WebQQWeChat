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
        protected int _retryTimes;
        protected virtual int MaxReTryTimes { get; set; } = 3;
        public IActionFuture ActionFuture { get; set; }
        protected readonly IHttpService _httpService;

        protected AbstractHttpAction(IHttpService httpService, ActionEventListener listener = null)
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

        public void NotifyActionEvent(ActionEventType type, object target = null)
        {
            NotifyActionEvent(new ActionEvent(type, target));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var requestItem = BuildRequest();
                await _httpService.ExecuteHttpRequestAsync(requestItem, ActionFuture?.Token ?? CancellationToken.None, this);
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

        public virtual async void OnHttpError(Exception ex)
        {
            if (++_retryTimes < MaxReTryTimes)
            {
                NotifyActionEvent(new ActionEvent(ActionEventType.EvtRetry, ex));
                if (ActionFuture != null) ActionFuture.ExcuteAction(this);
                else await ExecuteAsync();
            }
            else NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public event ActionEventListener OnActionEvent;
    }
}
