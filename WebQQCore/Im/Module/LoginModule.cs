using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class LoginModule : AbstractModule
    {
        public IQQActionFuture GetSelfInfo(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetSelfInfoAction(Context, listener));
        }

        public override QQModuleType GetModuleType()
        {
            return QQModuleType.LOGIN;
        }

        public IQQActionFuture GetQRCode(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetQRCodeAction(Context, listener));
        }

        public IQQActionFuture CheckQRCode(QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckQRCodeAction(Context, listener));
        }

        public IQQActionFuture CheckVerify(string qqAccount, QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckVerifyAction(Context, listener, qqAccount));
        }

        public IQQActionFuture WebLogin(string username, string password, long uin,
            string verifyCode, QQActionEventHandler listener)
        {
            return PushHttpAction(new WebLoginAction(Context, listener, username, password, uin, verifyCode));
        }

        public IQQActionFuture ChannelLogin(QQStatus status, QQActionEventHandler listener)
        {
            return PushHttpAction(new ChannelLoginAction(Context, listener, status));
        }

        public IQQActionFuture GetCaptcha(long uin, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetCaptchaImageAction(Context, listener, uin));
        }

        public IQQActionFuture PollMsg(QQActionEventHandler listener)
        {
            return PushHttpAction(new PollMsgAction(Context, listener));
        }

        public IQQActionFuture Logout(QQActionEventHandler listener)
        {
            return PushHttpAction(new WebLogoutAction(Context, listener));
        }

        public IQQActionFuture GetLoginSig(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetLoginSigAction(Context, listener));
        }

        public IQQActionFuture CheckLoginSig(string checkUrl, QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckLoginSigAction(Context, listener, checkUrl));
        }

        public IQQActionFuture GetVFWebqq(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetVFWebqq(Context, listener));
        }

    }

}
