using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FclEx;
using FclEx.Http.Actions;
using FclEx.Http.Event;
using FclEx.Http.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Modules.Impl;

namespace WebQQ.Im.Actions
{
    public abstract class WebQQAction : CommonHttpAction
    {
        // 为了防止通知层级混乱，其他action不应该直接操作Context，本action也只是在报告错误时用到了。
        // 其他通知应该先通知到调用action的模块，由模块决定是否需要进一步通知
        private readonly IQQContext _context;
        private static readonly ConcurrentDictionary<Type, string> _urlApiDic 
            = new ConcurrentDictionary<Type, string>();

        protected override string Url => GetUrl(_urlApiDic, typeof(ApiUrls));
        protected ILogger Logger => _context.Logger;
        protected SessionModule Session => _context.GetModule<SessionModule>();
        protected StoreModule Store => _context.GetModule<StoreModule>();
        protected long Timestamp => DateTime.Now.ToTimestampMilli();

        protected WebQQAction(IQQContext context, ActionEventListener listener = null)
            : base(context.Http)
        {
            _context = context;
            OnActionEvent += listener;
        }

        protected override ValueTask<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            var exception = ex as QQException ?? new QQException(ex);
            return base.HandleExceptionAsync(exception);
        }

        protected ValueTask<ActionEvent> NotifyErrorEventAsync(QQException ex)
        {
            return NotifyActionEventAsync(ActionEventType.EvtError, ex);
        }

        protected ValueTask<ActionEvent> NotifyErrorEventAsync(QQErrorCode code)
        {
            return NotifyErrorEventAsync(QQException.CreateException(code));
        }

        protected ValueTask<ActionEvent> NotifyErrorEventAsync(QQErrorCode code, string msg)
        {
            return NotifyErrorEventAsync(QQException.CreateException(code, msg));
        }

        protected override async ValueTask<ActionEvent> ExecuteInternalAsync(CancellationToken token)
        {
            Logger.LogTrace($"[Action={ActionName} Begin]");
            var result = await base.ExecuteInternalAsync(token).ConfigureAwait(false);
            Logger.LogTrace($"[Action={ActionName} End]");
            return result;
        }

        protected override async ValueTask<ActionEvent> NotifyActionEventAsync(ActionEvent actionEvent)
        {
            var type = actionEvent.Type;
            var typeName = type.GetDescription();
            var target = actionEvent.Target;

            switch (type)
            {
                case ActionEventType.EvtError:
                {
                    var ex = (Exception)target;
                    var msg = ex.ToString().TrimEnd();
                    Logger.LogError($"[Action={ActionName}, Result={typeName}, {msg}]");
                    await _context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.Error, ex));
                    break;
                }

                case ActionEventType.EvtRetry:
                {
                    var ex = (Exception)target;
                    Logger.LogWarning($"[Action={ActionName}, Result={typeName}, RetryTimes={ErrorTimes}][{ex.Message}]");
                    break;
                }

                case ActionEventType.EvtCanceled:
                    Logger.LogWarning($"[Action={ActionName}, Result={typeName}, Target={target}]");
                    break;

                case ActionEventType.EvtOk:
                    Logger.LogTrace($"[Action={ActionName}, Result={typeName}]");
                    break;

                default:
                    Logger.LogDebug($"[Action={ActionName}, Result={typeName}]");
                    break;
            }
            return await base.NotifyActionEventAsync(actionEvent);
        }
    }
}
