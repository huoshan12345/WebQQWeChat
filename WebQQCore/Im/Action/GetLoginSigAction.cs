using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取从登陆页面获取LoginSig</para>
    /// <para>@author solosky</para>
    /// <para>接口更新 2013-08-03</para>
    /// </summary>
    public class GetLoginSigAction : AbstractHttpAction
    {
        public GetLoginSigAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            return CreateHttpRequest(HttpConstants.Get, QQConstants.URL_LOGIN_PAGE);
        }
        
        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var rex = new Regex(QQConstants.REGXP_LOGIN_SIG);
            var mc = rex.Match(response.GetResponseString());

            if (mc.Success)
            {
                var session = Context.Session;
                session.LoginSig = mc.Groups[1].Value;

                NotifyActionEvent(QQActionEventType.EVT_OK, session.LoginSig);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.INVALID_RESPONSE, "Login Sig Not Found!!"));
            }
        }

    }

}
