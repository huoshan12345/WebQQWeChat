using System.Collections.Generic;
using System.Drawing;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Util.Log;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// 处理整体登陆逻辑
    /// </summary>
    public class ProcModule : AbstractModule
    {
        public QQActionFuture Login(QQActionEventHandler listener)
        {
            var future = new ProcActionFuture(listener, true);
            // DoGetLoginSig(future); // 这里可以直接替换成 DoCheckVerify(future);
            DoCheckVerify(future);
            return future;
        }

        public QQActionFuture LoginWithVerify(string verifyCode, ProcActionFuture future)
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
            if (future.IsCanceled)
            {
                return;
            }
            var account = Context.Account;
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetCaptcha(account.Uin, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    var verify = new QQNotifyEventArgs.ImageVerify();

                    verify.Type = QQNotifyEventArgs.ImageVerify.VerifyType.LOGIN;
                    verify.Image = (Image)Event.Target;
                    verify.Reason = reason;
                    verify.Future = future;

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
                    var args = (QQActionEventArgs.CheckVerifyArgs)(Event.Target);
                    Context.Account.Uin = args.uin;
                    Context.Session.CapCd = args.code;
                    if (args.result == 0)
                    {
                        DoWebLogin(args.code, future);
                    }
                    else
                    {
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
            login.ChannelLogin(Context.Account.Status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_OK, null);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    future.NotifyActionEvent(QQActionEventType.EVT_ERROR, Event.Target);
                }
            });
        }

        public QQActionFuture Relogin(QQStatus status, QQActionEventHandler listener)
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
                    }
                    else if (session.State != QQSessionState.KICKED)
                    {
                        Relogin();
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    var session = Context.Session;
                    var account = Context.Account;
                    session.State = QQSessionState.OFFLINE;

                    //因为自带了错误重试机制，如果出现了错误回调，表明已经超时多次均失败，这里直接返回网络错误的异常
                    var ex = (QQException)Event.Target;
                    var code = ex.ErrorCode;
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
                        // LOG.warn("poll msg unexpected error, ignore it ...", ex);
                        Relogin();
                        DoPollMsg();
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_RETRY)
                {
                    // System.err.println("Poll Retry:" + this);
                    MyLogger.Default.Info("poll msg error, retrying....", (QQException) Event.Target);
                }
            });
        }

        /// <summary>
        /// 退出，下线
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public QQActionFuture DoLogout(QQActionEventHandler listener)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            return login.Logout(listener);
        }
    }

}
