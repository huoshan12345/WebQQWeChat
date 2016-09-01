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
    /// <para>在线好友</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetOnlineFriendAction : AbstractHttpAction
    {
        public GetOnlineFriendAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_ONLINE_BUDDY_LIST);
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JArray>();
                for (var i = 0; i < result.Count; i++)
                {
                    var obj = result[i].ToObject<JObject>();
                    var uin = obj["uin"].ToObject<long>();
                    var status = obj["status"].ToString();
                    var clientType = obj["client_type"].ToObject<int>();
                    var buddy = store.GetBuddyByUin(uin);
                    buddy.Status = QQStatus.ValueOfRaw(status);
                    buddy.ClientType = QQClientType.ValueOfRaw(clientType);
                }

            }

            NotifyActionEvent(QQActionEventType.EvtOK, store.GetOnlineBuddyList());
        }

    }

}
