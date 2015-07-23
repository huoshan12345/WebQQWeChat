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
        private QQUser user;
 
        public ShakeWindowAction(QQContext context, QQActionEventHandler listener, QQUser user)
            : base(context, listener)
        {

            this.user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_SHAKE_WINDOW);
            req.AddGetValue("to_uin", user.Uin + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("t", DateUtils.NowTimestamp() + "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EVT_OK, user);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }

        }
    }

}
