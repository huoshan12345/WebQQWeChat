using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FxUtility.Extensions;
using HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Action.ActionResult;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;
using HttpAction;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule : WeChatModule, ILoginModule
    {
        public LoginModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
                .PushAction<GetUuidAction>()
                .PushAction<GetQRCodeAction>(async (sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;

                    var verify = (Image)@event.Target;
                    await Context.FireNotifyAsync(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.QRCodeReady, verify));
                })
                .PushAction<WatiForLoginAction>(async (sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;

                    var result = (WatiForLoginResult)@event.Target;
                    switch (result)
                    {
                        case WatiForLoginResult.Success:
                            await Context.FireNotifyAsync(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.QRCodeSuccess));
                            break;
                        case WatiForLoginResult.QRCodeInvalid:
                            await Context.FireNotifyAsync(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.QRCodeInvalid));
                            @event.Type = ActionEventType.EvtError; // 令后续动作不再执行
                            break;
                        case WatiForLoginResult.ScanCode:
                            @event.Type = ActionEventType.EvtRepeat;
                            break;
                    }
                })
                .PushAction<WebLoginAction>()
                .PushAction<WebwxInitAction>()
                .PushAction<StatusNotifyAction>()
                .PushAction<GetContactAction>(async (sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;

                    await Context.FireNotifyAsync(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.LoginSuccess));
                })
                .ExecuteAsync();
        }

        public void BeginSyncCheck()
        {
            var sync = new SyncCheckAction(Context);
            var wxSync = new WebwxSyncAction(Context, async (s, e) =>
            {
                if (e.Type == ActionEventType.EvtRetry) return;
                sync.ExecuteAsync().Forget();
                if (e.Type == ActionEventType.EvtOK)
                {
                    var msgs = (IList<Message>)e.Target;
                    // if (msgs.Count == 0) await Task.Delay(5 * 1000);
                    foreach (var msg in msgs)
                    {
                        var notify = WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.Message, msg);
                        await Context.FireNotifyAsync(notify);
                    }
                }
            });

            sync.OnActionEvent += (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtError)
                {
                    Context.GetModule<SessionModule>().State = SessionState.Offline;
                    Context.FireNotify(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.Offline));
                }
                else if (@event.Type == ActionEventType.EvtOK)
                {
                    var result = (SyncCheckResult)@event.Target;
                    switch (result)
                    {
                        case SyncCheckResult.Offline:
                        case SyncCheckResult.Kicked:
                            Context.FireNotify(WeChatNotifyEvent.CreateEvent(WeChatNotifyEventType.Offline));
                            return Task.CompletedTask;

                        case SyncCheckResult.UsingPhone:
                        case SyncCheckResult.NewMsg:
                            break;

                        case SyncCheckResult.RedEnvelope:
                        case SyncCheckResult.Nothing:
                            break;
                    }
                    (result == SyncCheckResult.Nothing ? sender : wxSync).ExecuteAsyncAuto().Forget();
                }
                return Task.CompletedTask;
            };

            sync.ExecuteAsyncAuto().Forget();
        }
    }
}
