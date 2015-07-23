using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// 接受别人的加好友请求
    /// </summary>
    public class AcceptBuddyAddAction : AbstractHttpAction
    {
        private readonly string _account;

        public AcceptBuddyAddAction(QQContext context, QQActionEventHandler listener, string account)
            : base(context, listener)
        {
            this._account = account;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_ACCEPET_BUDDY_ADD);

            JObject json = new JObject();
            json.Add("account", _account);
            json.Add("gid", "0");
            json.Add("mname", "");
            json.Add("vfwebqq", session.Vfwebqq);
            req.AddPostValue("r", json.ToString());
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EVT_OK, json);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                    new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }
        }

    }
}
