using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>震动聊天窗口</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class ShakeWindowAction : AbstractHttpAction
    {
        private readonly QQUser _user;
 
        public ShakeWindowAction(IQQContext context, QQActionListener listener, QQUser user)
            : base(context, listener)
        {

            _user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_SHAKE_WINDOW);
            req.AddGetValue("to_uin", _user.Uin);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EvtOK, _user);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.UnexpectedResponse));
            }

        }
    }

}
