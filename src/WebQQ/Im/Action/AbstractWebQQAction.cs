using System;
using HttpActionFrame.Action;
using HttpActionFrame.Event;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Service.Http;
using WebQQ.Im.Service.Log;

namespace WebQQ.Im.Action
{
    public abstract class AbstractWebQQAction : AbstractHttpAction
    {
        protected readonly IQQContext _context;
        protected readonly IQQLogger _logger;

        protected AbstractWebQQAction(IQQContext context, ActionEventListener listener)
            : base(context.GetSerivce<IQQHttpService>(QQServiceType.Http), listener)
        {
            _context = context;
            _logger = context.GetSerivce<IQQLogger>(QQServiceType.Log);
        }

        public override void NotifyActionEvent(ActionEvent actionEvent)
        {
            var type = actionEvent.Type;
            var target = actionEvent.Target;

            switch (type)
            {
                case ActionEventType.EvtError:
                {
                    var ex = (QQException)target;
                    _logger.LogError($"[Action={GetType().Name}, Type={type}, {ex}");
                    _context.FireNotify(new QQNotifyEvent(QQNotifyEventType.NetError, ex));
                    break;
                }
                case ActionEventType.EvtRetry:
                {
                    var ex = (QQException)target;
                    _logger.LogWarning($"[Action={GetType().Name}, Type={type}, RetryTimes={_retryTimes}][{ex.ToSimpleString()}]");
                    break;
                }
                case ActionEventType.EvtCanceled:
                {
                    _logger.LogInformation($"[Action={GetType().Name}, Type={type}, Target={target}]");
                    break;
                }
                default:
                {
                    _logger.LogDebug($"[Action={GetType().Name}, Type={type}");
                    break;
                }
            }
            base.NotifyActionEvent(actionEvent);
        }

        public override void OnHttpError(Exception ex)
        {
            var qqEx = ex as QQException ?? new QQException(ex);
            base.OnHttpError(qqEx);
        }
    }
}
