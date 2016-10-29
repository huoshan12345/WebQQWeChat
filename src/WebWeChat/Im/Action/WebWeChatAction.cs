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
        protected IWeChatContext Context { get; set; }
        protected ILoggerModule Logger { get; set; }
        protected SessionModule Session { get; set; }
        protected StoreModule Store { get; set; }
        protected AccountModule Account { get; set; }

        protected long Timestamp => DateTime.Now.ToTimestamp();
        protected string ActionName => GetType().Name;

        /// <summary>
        /// 把各模块交给ActionFuture初始化
        /// 即通过调用SetContext方法
        /// </summary>
        /// <param name="listener"></param>
        protected WebWeChatAction(ActionEventListener listener) : base(null, listener) { }

        protected WebWeChatAction(IWeChatContext context, ActionEventListener listener = null) :
            base(context.GetSerivce<IHttpService>(), listener)
        {
            SetContext(context);
        }

        public void SetContext(IWeChatContext context)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            HttpService = HttpService ?? context.GetSerivce<IHttpService>();
            Logger = Logger ?? context.GetModule<ILoggerModule>();
            Session = Session ?? context.GetModule<SessionModule>();
            Store = Store ?? context.GetModule<StoreModule>();
            Account = Account ?? context.GetModule<AccountModule>();
        }

        public override void OnHttpError(Exception ex)
        {
            var exception = ex as WeChatException ?? new WeChatException(ex);
            base.OnHttpError(exception);
        }

        protected void NotifyErrorEvent(WeChatException ex)
        {
            NotifyActionEvent(new ActionEvent(ActionEventType.EvtError, ex));
        }

        protected void NotifyErrorEvent(WeChatErrorCode code)
        {
            NotifyErrorEvent(WeChatException.CreateException(code));
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
