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

        protected virtual void NotifyActionEvent(ActionEvent actionEvent)
        {
            OnActionEvent?.Invoke(this, actionEvent);
        }

        protected virtual void NotifyActionEvent(ActionEventType type, object target = null)
        {
            NotifyActionEvent(new ActionEvent(type, target));
        }

        public virtual void OnHttpHeader(HttpResponseItem responseItem)
        {
        }

        public virtual void OnHttpContent(HttpResponseItem responseItem)
        {
        }

        public virtual void OnHttpError(Exception ex)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public abstract HttpRequestItem BuildRequest();

        public event ActionEventListener OnActionEvent;

        public virtual async Task<ActionEventType> ExecuteAsync(CancellationToken token)
        {
            ++ExcuteTimes;
            for (var i = 0; i < MaxReTryTimes; i++)
            {
                try
                {
                    var requestItem = BuildRequest();
                    await HttpService.ExecuteHttpRequestAsync(requestItem, token, this);
                    return ActionEventType.EvtOK;
                }
                catch (TaskCanceledException)
                {
                    NotifyActionEvent(new ActionEvent(ActionEventType.EvtCanceled, this));
                    return ActionEventType.EvtCanceled;
                }
                catch (OperationCanceledException)
                {
                    NotifyActionEvent(new ActionEvent(ActionEventType.EvtCanceled, this));
                    return ActionEventType.EvtCanceled;
                }
                catch (Exception ex)
                {
                    if (i + 1 < MaxReTryTimes)
                    {
                        NotifyActionEvent(new ActionEvent(ActionEventType.EvtRetry, ex));
                        return ActionEventType.EvtRetry;
                    }
                    else
                    {
                        OnHttpError(ex);
                    }
                }
            }
            return ActionEventType.EvtError;
        }
    }
}
