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

        public SendInputNotifyAction(IQQContext context, QQActionEventHandler listener, QQUser user)
            : base(context, listener)
        {

            this.user = user;
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
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                        new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }
        }
 
        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_SEND_INPUT_NOTIFY);
            QQSession session = Context.Session;
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("to_uin", user.Uin + "");
            req.AddGetValue("t", DateUtils.NowTimestamp() + "");
            req.AddGetValue("psessionid", session.SessionId);
            return req;
        }
    }

}
