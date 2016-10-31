using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule : WeChatModule, ILoginModule
    {
        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            var future = new WebWeChatActionFuture(Context, listener)
                .PushAction(new GetUuidAction(Context))
                .PushAction(new GetQRCodeAction(Context, (sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        var verify = (Image)@event.Target;
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeReady, verify));
                    }
                }))
                .PushAction(new WatiForLoginAction(Context, (sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtRepeat)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeSuccess));
                    }
                    else if (@event.Type == ActionEventType.EvtError)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeInvalid));
                    }
                }))
                .PushAction(new WebLoginAction(Context, (sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.LoginSuccess));
                        AfterLogin();
                        BeginSyncCheck();
                    }
                }));
            return future.ExecuteAsync(CancellationToken.None);
        }

        private void AfterLogin()
        {
            var future = new WebWeChatActionFuture(Context)
                .PushAction(new WebwxInitAction(Context))
                .PushAction(new StatusNotifyAction(Context))
                .PushAction(new GetContactAction(Context))
                .PushAction(new BatchGetContactAction())
                .ExecuteAsync(CancellationToken.None);
        }

        private void BeginSyncCheck()
        {
            var future = new WebWeChatActionFuture(Context);
            future.PushAction(new SyncCheckAction((sender, @event) =>
            {
                if (@event.Type != ActionEventType.EvtOK) return;
                var result = (SyncCheckResult) @event.Target;
                switch (result)
                {
                    case SyncCheckResult.Nothing:
                        break;
                    case SyncCheckResult.NewMsg:
                        break;
                    case SyncCheckResult.UsingPhone:
                        break;
                    case SyncCheckResult.RedEnvelope:
                        break;
                    case SyncCheckResult.Offline:
                        break;
                    case SyncCheckResult.Kicked:
                        break;
                }
                if (Context.GetModule<SessionModule>().State == SessionState.Online)
                {
                    future.PushAction(sender);
                }
            })).ExecuteAsync(CancellationToken.None);
        }

        public LoginModule(IWeChatContext context) : base(context)
        {
        }
    }
}
