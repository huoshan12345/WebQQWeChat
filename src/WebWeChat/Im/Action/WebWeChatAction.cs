using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Impl;
using FxUtility.Extensions;
using HttpAction.Action;
using HttpAction.Event;
using HttpAction.Service;

namespace WebWeChat.Im.Action
{
    public abstract class WebWeChatAction : AbstractHttpAction
    {
        // 为了防止通知层级混乱，其他action不应该直接操作Context，本action也只是在报告错误时用到了。
        // 其他通知应该先通知到调用action的模块，由模块决定是否需要进一步通知
        private IWeChatContext Context { get; set; }

        protected ILogger Logger => Context.GetSerivce<ILogger>();
        protected SessionModule Session => Context.GetModule<SessionModule>();
        protected StoreModule Store => Context.GetModule<StoreModule>();
        protected AccountModule Account => Context.GetModule<AccountModule>();
        protected IConfigurationRoot Config => Context.GetSerivce<IConfigurationRoot>();

        protected long Timestamp => DateTime.Now.ToTimestampMilli();
        protected string ActionName => GetType().GetDescription();

        protected WebWeChatAction(IWeChatContext context, ActionEventListener listener = null) :
            base(context.GetSerivce<IHttpService>())
        {
            Context = context;
            OnActionEvent += listener;
        }

        public override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            var exception = ex as WeChatException ?? new WeChatException(ex);
            return base.HandleExceptionAsync(exception);
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(WeChatException ex)
        {
            return NotifyActionEventAsync(ActionEventType.EvtError, ex);
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(WeChatErrorCode code)
        {
            return NotifyErrorEventAsync(WeChatException.CreateException(code));
        }

        protected Task<ActionEvent> NotifyErrorEventAsync(WeChatErrorCode code, string msg)
        {
            return NotifyErrorEventAsync(WeChatException.CreateException(code, msg));
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
                        var ex = (WeChatException)target;
                        Logger.LogError($"[Action={ActionName}, Result={typeName}, {ex}");
                        await Context.FireNotifyAsync(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.Error, ex));
                        break;
                    }
                case ActionEventType.EvtRetry:
                    {
                        var ex = (WeChatException)target;
                        Logger.LogWarning($"[Action={ActionName}, Result={typeName}, RetryTimes={RetryTimes}][{ex.ToSimpleString()}]");
                        break;
                    }
                case ActionEventType.EvtCanceled:
                    Logger.LogWarning($"[Action={ActionName}, Result={typeName}, Target={target}]");
                    break;

                default:
                    Logger.LogInformation($"[Action={ActionName}, Result={typeName}]");
                    break;
            }
            return await base.NotifyActionEventAsync(actionEvent);
        }
    }
}
