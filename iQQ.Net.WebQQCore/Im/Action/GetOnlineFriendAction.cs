using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>在线好友</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetOnlineFriendAction : AbstractHttpAction
    {
        public GetOnlineFriendAction(QQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_GET_ONLINE_BUDDY_LIST);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            QQStore store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                JArray result = json["result"].ToObject<JArray>();
                for (int i = 0; i < result.Count; i++)
                {
                    JObject obj = result[i].ToObject<JObject>();
                    long uin = obj["uin"].ToObject<long>();
                    string status = obj["status"].ToString();
                    int clientType = obj["client_type"].ToObject<int>();

                    QQBuddy buddy = store.GetBuddyByUin(uin);
                    buddy.Status = QQStatus.ValueOfRaw(status);
                    buddy.ClientType = QQClientType.ValueOfRaw(clientType);
                }

            }

            NotifyActionEvent(QQActionEventType.EVT_OK, store.GetOnlineBuddyList());
        }

    }

}
