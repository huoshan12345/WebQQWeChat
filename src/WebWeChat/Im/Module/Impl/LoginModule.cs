using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction;
using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Action.ActionResult;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;
using WebWeChat.Im.Service.Impl;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule : WeChatModule, ILoginModule
    {
        public LoginModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            var future = new WeChatActionFuture(Context, listener)
                .PushAction<GetUuidAction>()
                .PushAction<GetQRCodeAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        var verify = (Image)@event.Target;
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeReady, verify));
                    }
                })
                .PushAction<WatiForLoginAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtRepeat)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeSuccess));
                    }
                    else if (@event.Type == ActionEventType.EvtError)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeInvalid));
                    }
                })
                .PushAction<WebLoginAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.LoginSuccess));
                        AfterLogin();
                    }
                });
            return future.ExecuteAsync();
        }

        private void AfterLogin()
        {
            var future = new WeChatActionFuture(Context)
                .PushAction<WebwxInitAction>()
                .PushAction<StatusNotifyAction>()
                .PushAction<GetContactAction>((sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;
                    BeginSyncCheck();
                })
                .PushAction<BatchGetContactAction>()
                .ExecuteAsync();
        }

        public void BeginSyncCheck()
        {
            var sync = new SyncCheckAction(Context);
            var wxSync = new WebwxSyncAction(Context, async (s, e) =>
            {
                if (e.Type == ActionEventType.EvtRetry) return;

                if (e.Type == ActionEventType.EvtOK)
                {
                    var msgs = (List<Message>)e.Target;
                    if (msgs.Count == 0) await Task.Delay(3 * 1000);

                    foreach (var msg in msgs.Where(m => m.MsgType != MessageType.GetContact))
                    {
                        var notify = new WeChatNotifyEvent(WeChatNotifyEventType.Message, msg);
                        await Context.FireNotifyAsync(notify);
                    }
                }
                Dispatcher.PushActor(sync);
            });

            sync.OnActionEvent += async (sender, @event) =>
            {
                if (@event.Type != ActionEventType.EvtOK) return;
                var result = (SyncCheckResult)@event.Target;
                switch (result)
                {
                    case SyncCheckResult.Offline:
                    case SyncCheckResult.Kicked:
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.Offline));
                        break;

                    case SyncCheckResult.UsingPhone:
                    case SyncCheckResult.NewMsg:
                        Dispatcher.PushActor(wxSync);
                        break;

                    case SyncCheckResult.RedEnvelope:
                    case SyncCheckResult.Nothing:
                        await Task.Delay(3 * 1000);
                        Dispatcher.PushActor(sender);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            Dispatcher.PushActor(sync);
        }
    }
}
