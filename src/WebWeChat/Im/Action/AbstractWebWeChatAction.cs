using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Action
{
    public abstract class AbstractWebWeChatAction : AbstractHttpAction
    {
        protected ILogger Logger { get; }
        protected IWeChatContext Context { get; }
        protected SessionModule Session { get; }
        protected long Timestamp => DateTime.Now.ToTimestamp();

        protected AbstractWebWeChatAction(IWeChatContext context, ActionEventListener listener = null) :
            base(context.GetSerivce<IHttpService>(), listener)
        {
            Context = context;
            Session = context.GetModule<SessionModule>();
            Logger = context.GetModule<ILoggerModule>();
        }

        public override void OnHttpError(Exception ex)
        {
            var qqEx = ex as WeChatException ?? new WeChatException(ex);
            base.OnHttpError(qqEx);
        }

        public void NotifyErrorEvent(WeChatException ex)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        public void NotifyErrorEvent(WeChatErrorCode code)
        {
            NotifyErrorEvent(new WeChatException(code));
        }

        public override void NotifyActionEvent(ActionEvent actionEvent)
        {
            var type = actionEvent.Type;
            var target = actionEvent.Target;

            switch (type)
            {
                case ActionEventType.EvtError:
                    {
                        var ex = (WeChatException)target;
                        Logger.LogError($"[Action={GetType().Name}, Type={type}, {ex}");
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.Error, ex));
                        break;
                    }
                case ActionEventType.EvtRetry:
                    {
                        var ex = (WeChatException)target;
                        Logger.LogWarning($"[Action={GetType().Name}, Type={type}, RetryTimes={_retryTimes}][{ex.ToSimpleString()}]");
                        break;
                    }
                case ActionEventType.EvtCanceled:
                    {
                        Logger.LogInformation($"[Action={GetType().Name}, Type={type}, Target={target}]");
                        break;
                    }
                default:
                    {
                        Logger.LogDebug($"[Action={GetType().Name}, Type={type}");
                        break;
                    }
            }
            base.NotifyActionEvent(actionEvent);
        }
    }
}
