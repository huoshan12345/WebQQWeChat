using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using Utility.HttpAction.Service;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Action
{
    public abstract class WebWeChatAction : HttpAction
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
        protected WebWeChatAction(ActionEventListener listener) : base(null)
        {
            OnActionEvent += listener;
        }

        protected WebWeChatAction(IWeChatContext context, ActionEventListener listener = null) :
            base(context.GetSerivce<IHttpService>())
        {
            OnActionEvent += listener;
            SetContext(context);
        }

        public void SetContext(IWeChatContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            HttpService = context.GetSerivce<IHttpService>();
            Logger = context.GetModule<ILoggerModule>();
            Session = context.GetModule<SessionModule>();
            Store = context.GetModule<StoreModule>();
            Account = context.GetModule<AccountModule>();
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

        public override async Task<ActionEventType> ExecuteAsync(CancellationToken token)
        {
            Logger.LogTrace($"[Action={ActionName} Begin]");
            var result = await base.ExecuteAsync(token);
            Logger.LogTrace($"[Action={ActionName} End]");
            return result;
        }

        protected override void NotifyActionEvent(ActionEvent actionEvent)
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
                        Logger.LogWarning($"[Action={ActionName}, Type={type}, RetryTimes={RetryTimes}][{ex.ToSimpleString()}]");
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
