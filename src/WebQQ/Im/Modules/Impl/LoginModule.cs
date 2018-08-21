using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FclEx;
using FclEx.Http;
using FclEx.Http.Event;
using WebQQ.Im.Actions;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Modules.Interface;
using WebQQ.Util;

namespace WebQQ.Im.Modules.Impl
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// </summary>
    internal class LoginModule : QQModule, ILoginModule
    {
        public void BeginPoll()
        {
            new PollMsgAction(Context, (sender, @event) => // 1.获取二维码
            {
                if (@event.TryGet<List<QQNotifyEvent>>(out var notifyEvents))
                {
                    foreach (var notifyEvent in notifyEvents)
                    {
                        switch (notifyEvent.Type)
                        {
                            case QQNotifyEventType.NeedUpdateFriends:
                                new GetFriendsAction(Context, (a, e) =>
                                {
                                    if (@event.IsOk)
                                    {
                                        new GetOnlineFriendsAction(Context)
                                        .ExecuteAutoAsync().Forget();
                                    }
                                    return Task.CompletedTask;
                                }).ExecuteAutoAsync().Forget();
                                break;

                            case QQNotifyEventType.NeedUpdateGroups:
                                //new GetGroupNameListAction(Context, (s, e) =>
                                //{
                                //    if (e.IsOk)
                                //    {
                                //        Store.GroupDic.Values.ForEachAsync(m => new GetGroupInfoAction(Context, m)
                                //        .ExecuteAutoAsync()).Forget();
                                //    }
                                //    return Task.CompletedTask;
                                //}).ExecuteAutoAsync().Forget();

                                new GetGroupInfoAction(Context, notifyEvent.Target.CastTo<QQGroup>()).ExecuteAutoAsync().Forget();
                                break;

                            default:
                                if (notifyEvent.Type >= 0) Context.FireNotifyAsync(notifyEvent).Forget(); // 仅通知大于0的
                                break;
                        }
                    }
                }
                return Task.CompletedTask;
            }).ExecuteForeverAsync(e => !Context.IsOnline()).Forget();
        }

        public LoginModule(IQQContext context) : base(context)
        {
        }

        public async ValueTask<ActionEvent> Login(ActionEventListener listener)
        {
            Session.State = SessionState.Logining;

            var loginFutureResult = await new WebQQActionFuture(Context, listener)
             .PushAction<GetQRCodeAction>(async (sender, @event) => // 1.获取二维码
             {
                 if (!@event.IsOk) return;
                 await Context.FireNotifyAsync(QQNotifyEventType.QRCodeReady, @event.Target);
             })
             .PushAction<CheckQRCodeAction>(async (sender, @event) => // 2.获取二维码扫描状态
             {
                 if (!@event.IsOk) return;

                 var args = (CheckQRCodeArgs)@event.Target;
                 switch (args.Status)
                 {
                     case QRCodeStatus.Ok:
                         Session.CheckSigUrl = args.Msg;
                         await Context.FireNotifyAsync(QQNotifyEventType.QRCodeSuccess);
                         break;

                     case QRCodeStatus.Valid:
                     case QRCodeStatus.Auth:
                         @event.Type = ActionEventType.EvtRepeat;
                         await Task.Delay(3000);
                         break;

                     case QRCodeStatus.Invalid:
                         await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.QRCodeInvalid, args.Msg));
                         @event.Type = ActionEventType.EvtCanceled;
                         break;
                 }
             })
             .PushAction<CheckSigAction>()
             .PushAction<GetVfwebqqAction>()
             .PushAction<ChannelLoginAction>(async (sender, @event) =>
             {
                 if (!@event.IsOk) return;
                 await Context.FireNotifyAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.LoginSuccess));
             }).ExecuteAutoAsync();

            if (!loginFutureResult.IsOk)
            {
                Session.State = SessionState.Offline;
            }
            else
            {
                Session.State = SessionState.Online;
                await GetClientInfoAfterLogin(listener);
            }

            return loginFutureResult;
        }

        private ValueTask<ActionEvent> GetClientInfoAfterLogin(ActionEventListener listener)
        {
            return new WebQQActionFuture(Context, listener)
                .PushAction<GetFriendsAction>(async (sender, @event) =>
                {
                    if (!@event.IsOk) return;
                    var obj = Store.FriendDic.FirstOrDefault().Value;
                    if (obj == null) return;
                    await new GetFriendLongNickAction(Context, obj).ExecuteAutoAsync();
                    //await new GetFriendQQNumberAction(Context, obj).ExecuteAutoAsync();
                    await new GetFriendInfoAction(Context, obj).ExecuteAutoAsync();
                })
                .PushAction<GetGroupNameListAction>((sender, @event) =>
                {
                    if (@event.IsOk)
                    {
                        Store.GroupDic.Values.ForEachAsync(m => new GetGroupInfoAction(Context, m).ExecuteAutoAsync()).Forget();
                    }
                    return Task.CompletedTask;
                })
                .PushAction<GetDiscussionListAction>((sender, @event) =>
                {
                    if (@event.IsOk)
                    {
                        Store.DiscussionDic.Values.ForEachAsync(m => new GetDiscussionInfoAction(Context, m).ExecuteAutoAsync()).Forget();
                    }
                    return Task.CompletedTask;
                })
                .PushAction<GetSelfInfoAction>()
                .PushAction<GetOnlineFriendsAction>()
                .ExecuteAutoAsync();
        }
    }
}
