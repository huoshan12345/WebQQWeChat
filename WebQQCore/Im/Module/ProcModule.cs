using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// 处理整体登陆逻辑
    /// </summary>
    public class ProcModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.PROC;
        }

        public IQQActionFuture LoginWithQRCode(QQActionEventHandler listener)
        {
            var future = new ProcActionFuture(listener, true);
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetQRCode((sender, @event) =>
            {
                if (@event.Type == QQActionEventType.EVT_OK)
                {
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QRCODE_READY, verify));
                    CheckQRCode(future);
                }
                else if (@event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, "获取二维码失败");
                }
            });
            return future;
        }

        private void CheckQRCode(ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            QQActionEventHandler handler = null;
            handler = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    var eventArgs = (CheckQRCodeArgs) Event.Target;
                    switch (eventArgs.Status)
                    {
                        case QRCodeStatus.QRCODE_OK:
                            Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QRCODE_SUCCESS));
                            DoCheckLoginSig(eventArgs.Msg, future);
                            break;

                        case QRCodeStatus.QRCODE_VALID:
                        case QRCodeStatus.QRCODE_AUTH:
                            Thread.Sleep(3000);
                            login.CheckQRCode(handler);
                            break;

                        case QRCodeStatus.QRCODE_INVALID:
                            Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QRCODE_INVALID, eventArgs.Msg));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            };
            login.CheckQRCode(handler);
        }


        public IQQActionFuture Login(QQActionEventHandler listener)
        {
            var future = new ProcActionFuture(listener, true);
            // DoGetLoginSig(future); // 这里可以直接替换成 DoCheckVerify(future);
            DoCheckVerify(future);
            return future;
        }

        public IQQActionFuture LoginWithVerify(string verifyCode, ProcActionFuture future)
        {
            DoWebLogin(verifyCode, future);
            return future;
        }

        private void DoGetLoginSig(ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetLoginSig((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    DoCheckVerify(future);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        private void DoGetVerify(string reason, ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var account = Context.Account;
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetCaptcha(account.Uin, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    var verify = new ImageVerify
                    {
                        Type = ImageVerify.VerifyType.LOGIN,
                        Image = (Image)Event.Target,
                        Reason = reason,
                        Future = future
                    };
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.CAPACHA_VERIFY, verify));
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        private void DoCheckVerify(ProcActionFuture future)
        {
            if (future.IsCanceled)
            {
                return;
            }

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            var account = Context.Account;
            login.CheckVerify(account.Username, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    var args = (CheckVerifyArgs)(Event.Target);
                    Context.Account.Uin = args.Uin;
                    if (args.Result == 0)
                    {
                        DoWebLogin(args.Code, future);
                    }
                    else
                    {
                        Context.Session.CapCd = args.Code;
                        DoGetVerify("为了保证您账号的安全，请输入验证码中字符继续登录。", future);
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        private void DoWebLogin(string verifyCode, ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.WebLogin(Context.Account.Username, Context.Account.Password, Context.Account.Uin,
                verifyCode, ((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    DoCheckLoginSig((string)Event.Target, future);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    var ex = (QQException)(Event.Target);
                    if (ex.ErrorCode == QQErrorCode.WRONG_CAPTCHA)
                    {
                        DoGetVerify(ex.Message, future);
                    }
                    else
                    {
                        future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                    }
                }
            }));
        }

        private void DoCheckLoginSig(string checkSigUrl, ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.CheckLoginSig(checkSigUrl, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    DoGetVFWebqq(future);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        private void DoGetVFWebqq(ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetVFWebqq((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    DoChannelLogin(future);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        private void DoChannelLogin(ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            if(Context.Account.Status == QQStatus.OFFLINE) Context.Account.Status = QQStatus.ONLINE;

            login.ChannelLogin(Context.Account.Status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_OK, null);
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.LOGIN_SUCCESS));
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        public IQQActionFuture Relogin(QQStatus status, QQActionEventHandler listener)
        {
            Context.Account.Status = status;
            Context.Session.State = QQSessionState.LOGINING;
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            MyLogger.Default.Info("iqq client Relogin...");
            var future = login.ChannelLogin(status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    MyLogger.Default.Info("iqq client ReloginChannel fail!!! use Relogin.");
                    Login(listener);
                }
                else
                {
                    listener(this, Event);
                }
            });
            return future;
        }

        public void Relogin()
        {
            MyLogger.Default.Info("Relogin...");
            var session = Context.Session;
            if (session.State == QQSessionState.LOGINING) return;
            // 登录失效，重新登录
            Relogin(Context.Account.Status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    // 重新登录成功重新POLL
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.RELOGIN_SUCCESS, null));
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.UNKNOWN_ERROR, Event));
                }
            });
        }

        /// <summary>
        /// 轮询新消息，发送心跳包，也就是挂qq
        /// </summary>
        public void DoPollMsg()
        {
            MyLogger.Default.Info("polling...");
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.PollMsg((sender, Event) =>
            {
                // 回调通知事件函数
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    var events = (List<QQNotifyEvent>)Event.Target;
                    foreach (var evt in events)
                    {
                        Context.FireNotify(evt);
                    }

                    // 准备提交下次poll请求
                    var session = Context.Session;
                    if (session.State == QQSessionState.ONLINE)
                    {
                        DoPollMsg();
                        return;
                    }
                    else if (session.State != QQSessionState.KICKED)
                    {
                        Relogin();
                        return;
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    //因为自带了错误重试机制，如果出现了错误回调，表明已经超时多次均失败，这里直接返回网络错误的异常
                    var ex = (QQException)Event.Target;
                    var code = ex.ErrorCode;
                    if (code == QQErrorCode.IO_TIMEOUT)
                    {
                        DoPollMsg();
                        return; // 心跳超时是正常的
                    }

                    var session = Context.Session;
                    var account = Context.Account;
                    session.State = QQSessionState.OFFLINE;


                    if (code == QQErrorCode.INVALID_LOGIN_AUTH)
                    {
                        Relogin();
                    }
                    else if (code == QQErrorCode.IO_ERROR || code == QQErrorCode.IO_TIMEOUT)
                    {
                        account.Status = QQStatus.OFFLINE;
                        //粗线了IO异常，直接报网络错误
                        Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex));
                    }
                    else
                    {
                        MyLogger.Default.Info("poll msg unexpected error, ignore it ...", ex);
                        Relogin();
                        DoPollMsg();
                        return;
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_RETRY)
                {
                    // System.err.println("Poll Retry:" + this);
                    MyLogger.Default.Info("poll msg error, retrying....", (QQException)Event.Target);
                }
            });
        }

        /// <summary>
        /// 退出，下线
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture DoLogout(QQActionEventHandler listener)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            return login.Logout(listener);
        }
    }

}
