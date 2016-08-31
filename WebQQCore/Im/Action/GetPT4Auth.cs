using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>pt4登录验证，获取到一个链接</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月24日</para>
    /// </summary>
    public class GetPT4Auth : AbstractHttpAction
    {
        public GetPT4Auth(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_PT4_AUTH);
            req.AddGetValue("daid", "4");
            req.AddGetValue("appid", "1");
            req.AddGetValue("auth_token", QQEncryptor.Time33(httpService.GetCookie("supertoken",
                QQConstants.URL_CHANNEL_LOGIN).Value));
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var rex = new Regex(QQConstants.REGXP_EMAIL_AUTH);
            var m = rex.Match(response.GetResponseString());

            if (m.Success)
            {
                var qqHex = m.Groups[2].Value;
                NotifyActionEvent(QQActionEventType.EVT_OK, qqHex);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                        new QQException(QQErrorCode.INVALID_LOGIN_AUTH));
            }
        }
    }

}
