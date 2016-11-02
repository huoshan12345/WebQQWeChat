using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using Utility.HttpAction.Service;

namespace Utility.HttpAction.Action
{
    public abstract class HttpAction : IHttpAction
    {
        protected virtual int MaxReTryTimes { get; set; } = 3;

        protected int ExcuteTimes { get; set; }
        protected int RetryTimes { get; set; }
        protected IHttpService HttpService { get; set; }

        protected HttpAction(IHttpService httpHttpService)
        {
            HttpService = httpHttpService;
        }

        protected virtual Task<ActionEvent> NotifyActionEventAsync(ActionEvent actionEvent)
        {
            return Task.Run(() =>
            {
                if (actionEvent.Type == ActionEventType.EvtRetry) ++RetryTimes;
                else RetryTimes = 0;
                OnActionEvent?.Invoke(this, actionEvent); // 这里不要异步执行
                return actionEvent;
            });
        }

        protected virtual Task<ActionEvent> NotifyActionEventAsync(ActionEventType type, object target = null)
        {
            return NotifyActionEventAsync(target == null ? ActionEvent.EmptyEvents[type] : ActionEvent.CreateEvent(type, target));
        }

        public abstract HttpRequestItem BuildRequest();

        public abstract Task<ActionEvent> HandleResponse(HttpResponseItem response);

        public event ActionEventListener OnActionEvent;

        public virtual Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            try
            {
                if (RetryTimes < MaxReTryTimes)
                {
                    var result = NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtRetry, ex));
                    return result;
                }
                else
                {
                    return NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtError, ex));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"throw an unhandled exception when excute [{nameof(HandleExceptionAsync)}] method.", e);
            }

        }

        public virtual async Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
#if DEBUG
                HttpRequestItem req = null;
#endif
                ++ExcuteTimes;
                try
                {
                    var requestItem = BuildRequest();
#if DEBUG
                    req = requestItem;
#endif
                    var response = await HttpService.ExecuteHttpRequestAsync(requestItem, token).ConfigureAwait(false);
                    return await HandleResponse(response).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
#if DEBUG
                    // 此处用于生成请求信息，然后用fiddler等工具测试
                    if (req?.RawUrl.Contains("webwxsync") == true || req?.RawUrl.Contains("synccheck") == true)
                    {
                        var url = req.RawUrl;
                        var header = req.GetRequestHeader(HttpService.GetCookies(req.RawUrl));
                        var data = req.RawData;
                        var len = data.Length;
                    }
#endif
                    return await HandleExceptionAsync(ex).ConfigureAwait(false);
                }
            }
            return await NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this)).ConfigureAwait(false);
        }
    }
}
