using System;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>登录退出</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class WebLogoutAction : AbstractHttpAction
    {
        public WebLogoutAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;

            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_LOGOUT);
            req.AddGetValue("ids", ""); // 产生过会话才出现ID，如何获取？？
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var isOK = json["result"].ToString().ToLower();
            if (json["retcode"].ToString() == "0")
            {
                if (isOK.Equals("ok"))
                {
                    NotifyActionEvent(QQActionEventType.EVT_OK, isOK);
                    return;
                }
            }

            NotifyActionEvent(QQActionEventType.EVT_ERROR, isOK);
        }

    }

}
