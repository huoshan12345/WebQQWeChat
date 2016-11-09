using System;
using System.Drawing;
using System.Threading.Tasks;
using FxUtility.Extensions;
using HttpAction;
using HttpAction.Event;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Action;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Interface;

namespace WebQQ.Im.Module.Impl
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// </summary>
    public class LoginModule : QQModule, ILoginModule
    {
        public void BeginPoll()
        {
            throw new NotImplementedException();
        }

        public LoginModule(IQQContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener)
        {
            return new QQActionFuture(Context, listener)
                .PushAction<GetQRCodeAction>(async (sender, @event) => // 1.获取二维码
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        var verify = (Image) @event.Target;
                        await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.QRCodeReady, verify));
                    }
                })
                .PushAction<CheckQRCodeAction>(async (sender, @event) => // 2.获取二维码扫描状态
                {
                    if (@event.Type != ActionEventType.EvtOK) return;

                    var args = (CheckQRCodeArgs)@event.Target;
                    switch (args.Status)
                    {
                        case QRCodeStatus.OK:
                            Context.GetModule<SessionModule>().CheckSigUrl = args.Msg;
                            await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.QRCodeSuccess));
                            break;

                        case QRCodeStatus.Valid:
                        case QRCodeStatus.Auth:
                            Context.GetSerivce<ILogger>().LogDebug($"二维码状态：{args.Status.GetDescription()}");
                            @event.Type = ActionEventType.EvtRepeat;
                            await Task.Delay(3000);
                            break;

                        case QRCodeStatus.Invalid:
                            await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.QRCodeInvalid, args.Msg));
                            break;
                    }
                })
                .PushAction<CheckSigAction>()
                .PushAction<GetVfwebqqAction>()
                .PushAction<ChannelLoginAction>(async (sender, @event) =>
                {
                    if (@event.Type != ActionEventType.EvtOK) return;
                    await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.LoginSuccess));
                })
                .PushAction<GetFriendsAction>()
                .PushAction<GetGroupNameListAction>()
                .PushAction<GetDiscuzListAction>()
                .PushAction<GetSelfInfoAction>()
                .PushAction<GetOnlineBuddiesAction>()
                .ExecuteAsync();
        }
    }

}
