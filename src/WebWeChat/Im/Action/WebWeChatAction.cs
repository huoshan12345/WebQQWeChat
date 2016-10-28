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
    public abstract class WebWeChatAction : AbstractHttpAction
    {
        protected ILogger Logger { get; }
        protected IWeChatContext Context { get; }
        protected SessionModule Session { get; }
        protected StoreModule Store { get; }
        protected long Timestamp => DateTime.Now.ToTimestamp();
        protected string ActionName => GetType().Name;

        protected WebWeChatAction(IWeChatContext context, ActionEventListener listener = null) :
            base(context.GetSerivce<IHttpService>(), listener)
        {
            Context = context;
            Session = context.GetModule<SessionModule>();
            Logger = context.GetModule<ILoggerModule>();
            Store = context.GetModule<StoreModule>();
        }

        public override void OnHttpError(Exception ex)
        {
            var qqEx = ex as WeChatException ?? new WeChatException(ex);
            base.OnHttpError(qqEx);
        }

        protected void NotifyErrorEvent(WeChatException ex)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        protected void NotifyErrorEvent(WeChatErrorCode code)
        {
            NotifyErrorEvent(new WeChatException(code));
        }

        public override async Task ExecuteAsync()
        {
            Logger.LogTrace($"[Action={ActionName} Begin]");
            await base.ExecuteAsync();
            Logger.LogTrace($"[Action={ActionName} End]");
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
                        Logger.LogError($"[Action={ActionName}, Type={type}, {ex}");
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.Error, ex));
                        break;
                    }
                case ActionEventType.EvtRetry:
                    {
                        var ex = (WeChatException)target;
                        Logger.LogWarning($"[Action={ActionName}, Type={type}, RetryTimes={_retryTimes}][{ex.ToSimpleString()}]");
                        break;
                    }
                case ActionEventType.EvtCanceled:
                    Logger.LogInformation($"[Action={ActionName}, Type={type}, Target={target}]");
                    break;

                default:
                    Logger.LogDebug($"[Action={ActionName}, Type={type}]");
                    break;
            }
            base.NotifyActionEvent(actionEvent);
        }
    }
}
