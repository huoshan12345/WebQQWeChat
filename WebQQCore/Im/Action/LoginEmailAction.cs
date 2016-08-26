using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>登录邮箱</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月25日</para>
    /// </summary>
    public class LoginEmailAction : AbstractHttpAction
    {
        public LoginEmailAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_LOGIN_EMAIL);
            req.AddGetValue("fun", "passport");
            req.AddGetValue("from", "webqq");
            req.AddGetValue("Referer", "https://mail.qq.com/cgi-bin/loginpage");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            string REGXP = "sid=(.*?)\"";
            Regex rex = new Regex(REGXP);
            Match m = rex.Match(response.GetResponseString());

            if (m.Success)
            {
                string sid = m.Groups[1].Value;
                NotifyActionEvent(QQActionEventType.EVT_OK, sid);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, QQErrorCode.UNEXPECTED_RESPONSE);
            }
        }

    }

}
