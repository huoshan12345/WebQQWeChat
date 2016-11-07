using System;
using System.Threading;
using System.Threading.Tasks;
using FxUtility.Extensions;
using HttpAction.Action;
using HttpAction.Event;
using HttpAction.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;

namespace WebQQ.Im.Action
{
    public abstract class QQAction : AbstractHttpAction
    {
        // 为了防止通知层级混乱，其他action不应该直接操作Context，本action也只是在报告错误时用到了。
        // 其他通知应该先通知到调用action的模块，由模块决定是否需要进一步通知
        private IQQContext Context { get; set; }
        protected ILogger Logger { get; set; }
        protected SessionModule Session { get; set; }
        protected StoreModule Store { get; set; }
        protected AccountModule Account { get; set; }
        protected IConfigurationRoot Config { get; set; }

        protected long Timestamp => DateTime.Now.ToTimestampMilli();
        protected string ActionName => GetType().GetDescription();

        protected QQAction(IQQContext context, ActionEventListener listener = null)
            : base(context.GetSerivce<IHttpService>())
        {
            SetContext(context);
            OnActionEvent += listener;
        }

        public void SetContext(IQQContext context)
        {            
            if (context == Context) return;
            Context = context;
            HttpService = context.GetSerivce<IHttpService>();
            Logger = context.GetSerivce<ILogger>();
            Session = context.GetModule<SessionModule>();
            Store = context.GetModule<StoreModule>();
            Account = context.GetModule<AccountModule>();
            Config = context.GetSerivce<IConfigurationRoot>();
        }

        public override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            var exception = ex as QQException ?? new QQException(ex);
            return base.HandleExceptionAsync(exception);
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(QQException ex)
        {
            return NotifyActionEventAsync(ActionEventType.EvtError, ex);
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(QQErrorCode code)
        {
            return NotifyErrorEventAsync(QQException.CreateException(code));
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(QQErrorCode code, string msg)
        {
            return NotifyErrorEventAsync(QQException.CreateException(code, msg));
        }

        public override async Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            Logger.LogTrace($"[Action={ActionName} Begin]");
            var result = await base.ExecuteAsync(token).ConfigureAwait(false);
            Logger.LogTrace($"[Action={ActionName} End]");
            return result;
        }

        protected override async Task<ActionEvent> NotifyActionEventAsync(ActionEvent actionEvent)
        {
            var type = actionEvent.Type;
            var typeName = type.GetDescription();
            var target = actionEvent.Target;

            switch (type)
            {
                case ActionEventType.EvtError:
                    {
                        var ex = (QQException)target;
                        Logger.LogError($"[Action={ActionName}, Result={typeName}, {ex}");
                        await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.Error, ex));
                        break;
                    }
                case ActionEventType.EvtRetry:
                    {
                        var ex = (QQException)target;
                        Logger.LogWarning($"[Action={ActionName}, Result={typeName}, RetryTimes={RetryTimes}][{ex.ToSimpleString()}]");
                        break;
                    }
                case ActionEventType.EvtCanceled:
                    Logger.LogWarning($"[Action={ActionName}, Result={typeName}, Target={target}]");
                    break;

                default:
                    Logger.LogDebug($"[Action={ActionName}, Result={typeName}]");
                    break;
            }
            return await base.NotifyActionEventAsync(actionEvent);
        }
    }
}
