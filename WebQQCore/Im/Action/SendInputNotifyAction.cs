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
    /// <para>发送正在输入状态通知</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class SendInputNotifyAction : AbstractHttpAction
    {
        private QQUser user;

        public SendInputNotifyAction(IQQContext context, QQActionListener listener, QQUser user)
            : base(context, listener)
        {

            this.user = user;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EvtOK, user);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError,
                        new QQException(QQErrorCode.UnexpectedResponse, response.GetResponseString()));
            }
        }
 
        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_SEND_INPUT_NOTIFY);
            var session = Context.Session;
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("to_uin", user.Uin);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddGetValue("psessionid", session.SessionId);
            return req;
        }
    }

}
