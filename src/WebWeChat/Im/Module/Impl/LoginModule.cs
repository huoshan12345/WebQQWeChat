using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Action.ActionResult;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule : WeChatModule, ILoginModule
    {
        public LoginModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            return new WeChatActionFuture(Context, listener)
                .PushAction<GetUuidAction>()
                .PushAction<GetQRCodeAction>((sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeReady, verify));
                })
                .PushAction<WatiForLoginAction>((sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;
                    var result = (WatiForLoginResult) @event.Target;
                    switch (result)
                    {
                        case WatiForLoginResult.Success:
                            Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeSuccess));
                            break;
                        case WatiForLoginResult.QRCodeInvalid:
                            Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeInvalid));
                            break;
                        case WatiForLoginResult.ScanCode:
                            @event.Type = ActionEventType.EvtRepeat;
                            break;
                    }
                })
                .PushAction<WebLoginAction>()
                .PushAction<WebwxInitAction>()
                .PushAction<StatusNotifyAction>()
                .PushAction<GetContactAction>((sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.LoginSuccess));
                })
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
                    Dispatcher.PushActor(sync);
                    var msgs = (IList<Message>)e.Target;
                    // if (msgs.Count == 0) await Task.Delay(5 * 1000);
                    foreach (var msg in msgs)
                    {
                        var notify = new WeChatNotifyEvent(WeChatNotifyEventType.Message, msg);
                        await Context.FireNotifyAsync(notify);
                    }
                }
            });

            sync.OnActionEvent += (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtError)
                {
                    Context.GetModule<SessionModule>().State = SessionState.Offline;
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.Offline));
                    return;
                }
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
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Dispatcher.PushActor(sender);
            };

            Dispatcher.PushActor(sync);
        }
    }
}
