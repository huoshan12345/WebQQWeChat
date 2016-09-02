using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>登录邮箱</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月25日</para>
    /// </summary>
    public class LoginEmailAction : AbstractHttpAction
    {
        public LoginEmailAction(IQQContext context, QQActionListener listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_LOGIN_EMAIL);
            req.AddGetValue("fun", "passport");
            req.AddGetValue("from", "webqq");
            req.AddGetValue("Referer", "https://mail.qq.com/cgi-bin/loginpage");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var REGXP = "sid=(.*?)\"";
            var rex = new Regex(REGXP);
            var m = rex.Match(response.GetResponseString());

            if (m.Success)
            {
                var sid = m.Groups[1].Value;
                NotifyActionEvent(QQActionEventType.EvtOK, sid);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, QQErrorCode.UnexpectedResponse);
            }
        }

    }

}
