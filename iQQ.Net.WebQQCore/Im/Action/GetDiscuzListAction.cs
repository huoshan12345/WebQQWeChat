using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取讨论组列表</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetDiscuzListAction : AbstractHttpAction
    {
        public GetDiscuzListAction(QQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_DISCUZ_LIST);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() + "");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            //{"retcode":0,"result":{"dnamelist":[{"did":3536443553,"name":"\u8FD9\u662F\u6807\u9898"},
            //{"did":625885728,"name":""}],"dmasklist":[{"did":1000,"mask":0}]}}

            JObject json = JObject.Parse(response.GetResponseString());
            QQStore store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                JObject result = json["result"].ToObject<JObject>();
                JArray dizlist = result["dnamelist"].ToObject<JArray>();
                for (int i = 0; i < dizlist.Count; i++)
                {
                    QQDiscuz discuz = new QQDiscuz();
                    JObject dizjson = dizlist[i].ToObject<JObject>();
                    discuz.Did = dizjson["did"].ToObject<long>();
                    discuz.Name = dizjson["name"].ToString();
                    store.AddDiscuz(discuz);
                }
                NotifyActionEvent(QQActionEventType.EVT_OK, store.GetDiscuzList());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }
        }
    }

}
