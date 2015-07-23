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
    /// 改变在线状态
    /// </summary>
    public class ChangeStatusAction : AbstractHttpAction
    {
        private readonly QQStatus _status;

        public ChangeStatusAction(QQContext context, QQActionEventHandler listener, QQStatus status)
            : base(context, listener)
        {
            this._status = status;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = this.Context.Session;

            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_CHANGE_STATUS);
            req.AddGetValue("newstatus", _status.Value);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis() / 1000 + "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                Context.Account.Status = _status;
                NotifyActionEvent(QQActionEventType.EVT_OK, _status);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                    new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }
        }

    }

}
