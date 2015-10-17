using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class LoginModule : AbstractModule
    {

        public QQActionFuture GetQRCode(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetQRCodeAction(Context, listener));
        }

        public QQActionFuture CheckQRCode(QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckQRCodeAction(Context, listener));
        }

        public QQActionFuture CheckVerify(string qqAccount, QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckVerifyAction(Context, listener, qqAccount));
        }

        public QQActionFuture WebLogin(string username, string password, long uin,
            string verifyCode, QQActionEventHandler listener)
        {
            return PushHttpAction(new WebLoginAction(Context, listener, username, password, uin, verifyCode));
        }

        public QQActionFuture ChannelLogin(QQStatus status, QQActionEventHandler listener)
        {
            return PushHttpAction(new ChannelLoginAction(Context, listener, status));
        }

        public QQActionFuture GetCaptcha(long uin, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetCaptchaImageAction(Context, listener, uin));
        }

        public QQActionFuture PollMsg(QQActionEventHandler listener)
        {
            return PushHttpAction(new PollMsgAction(Context, listener));
        }

        public QQActionFuture Logout(QQActionEventHandler listener)
        {
            return PushHttpAction(new WebLogoutAction(Context, listener));
        }

        public QQActionFuture GetLoginSig(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetLoginSigAction(Context, listener));
        }

        public QQActionFuture CheckLoginSig(string checkUrl, QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckLoginSigAction(Context, listener, checkUrl));
        }

    }

}
