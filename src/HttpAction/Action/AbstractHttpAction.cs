using System;
using System.Threading;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;
using System.Linq;

namespace HttpAction.Action
{
    public abstract class AbstractHttpAction : IHttpAction
    {
        protected virtual int MaxReTryTimes { get; set; } = 3;

        protected int ExcuteTimes { get; set; }
        protected int RetryTimes { get; set; }
        protected IHttpService HttpService { get; set; }

        protected AbstractHttpAction(IHttpService httpHttpService)
        {
            HttpService = httpHttpService;
        }

        protected virtual async Task<ActionEvent> NotifyActionEventAsync(ActionEvent actionEvent)
        {
            try
            {
                if (OnActionEvent != null) await OnActionEvent.Invoke(this, actionEvent);
                if (actionEvent.Type == ActionEventType.EvtRetry) ++RetryTimes;
                else RetryTimes = 0;
                return actionEvent;
            }
            catch (Exception ex)
            {
                ++RetryTimes;
                return await HandleExceptionAsync(ex);
            }
        }

        protected Task<ActionEvent> NotifyActionEventAsync(ActionEventType type, object target = null)
        {
            return NotifyActionEventAsync(ActionEvent.CreateEvent(type, target));
        }

        protected Task<ActionEvent> NotifyOkActionEventAsync(object target = null)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK, target);
        }

        protected Task<ActionEvent> NotifyOkEventAsync(object target = null)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK, target);
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(Exception ex)
        {
            return NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtError, ex));
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(string msg)
        {
            return NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtError, new Exception(msg)));
        }

        public abstract HttpRequestItem BuildRequest();

        public abstract Task<ActionEvent> HandleResponse(HttpResponseItem response);

        public event ActionEventListener OnActionEvent;

        public virtual async Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            try
            {
                var @event = ActionEvent.CreateEvent(RetryTimes < MaxReTryTimes ?
                    ActionEventType.EvtRetry : ActionEventType.EvtError, ex);
                return await NotifyActionEventAsync(@event);
            }
            catch (Exception e)
            {
                throw new Exception($"throw an unhandled exception when excute [{nameof(HandleExceptionAsync)}] method.", e);
            }
        }

        public virtual async Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            HttpRequestItem requestItem = null;
            if (!token.IsCancellationRequested)
            {
                ++ExcuteTimes;
                try
                {
                    requestItem = BuildRequest();
                    var response = await HttpService.ExecuteHttpRequestAsync(requestItem, token).ConfigureAwait(false);
                    var result = await HandleResponse(response).ConfigureAwait(false);
                    // RetryTimes = 0;
                    return result;
                }
                catch (Exception ex)
                {
                    ex = ex.InnerException ?? ex;
#if DEBUG
                    // 此处用于生成请求信息，然后用fiddler等工具测试
                    if (requestItem != null)
                    {
                        var url = requestItem.RawUrl;
                        var header = requestItem.GetRequestHeader(HttpService.GetCookies(requestItem.RawUrl));
                        var data = requestItem.RawData;
                        var len = data.Length;
                    }
#endif
                    // ++RetryTimes;
                    return await HandleExceptionAsync(ex).ConfigureAwait(false);
                }
            }
            return await NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this)).ConfigureAwait(false);
        }
    }
}
