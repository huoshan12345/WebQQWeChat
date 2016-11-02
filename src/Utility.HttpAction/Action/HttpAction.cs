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

        protected virtual ActionEvent NotifyActionEvent(ActionEvent actionEvent)
        {
            OnActionEvent?.BeginInvoke(this, actionEvent, null, null);
            if (actionEvent.Type == ActionEventType.EvtRetry) ++RetryTimes;
            else RetryTimes = 0;
            return actionEvent;
        }

        protected virtual ActionEvent NotifyActionEvent(ActionEventType type, object target = null)
        {
            return NotifyActionEvent(target == null ? ActionEvent.EmptyEvents[type] : ActionEvent.CreateEvent(type, target));
        }

        public abstract HttpRequestItem BuildRequest();

        public abstract ActionEvent HandleResponse(HttpResponseItem response);

        public event ActionEventListener OnActionEvent;

        public virtual ActionEvent HandleException(Exception ex)
        {
            try
            {
                if (RetryTimes < MaxReTryTimes)
                {
                    var result = NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtRetry, ex));
                    return result;
                }
                else
                {
                    return NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtError, ex));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"throw an unhandled exception when excute [{nameof(HandleException)}] method.", e);
            }

        }

        public virtual async Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            ++ExcuteTimes;
            if (token.IsCancellationRequested)
            {
                return NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this));
            }
            try
            {
                var requestItem = BuildRequest();
                var response = await HttpService.ExecuteHttpRequestAsync(requestItem, token);
                return HandleResponse(response);
            }
            catch (TaskCanceledException)
            {
                return NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this));
            }
            catch (OperationCanceledException)
            {
                return NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
