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
    /// <para>获取讨论组列表</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetDiscuzListAction : AbstractHttpAction
    {
        public GetDiscuzListAction(IQQContext context, QQActionListener listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_DISCUZ_LIST);
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            //{"retcode":0,"result":{"dnamelist":[{"did":3536443553,"name":"\u8FD9\u662F\u6807\u9898"},
            //{"did":625885728,"name":""}],"dmasklist":[{"did":1000,"mask":0}]}}

            var json = JObject.Parse(response.GetResponseString());
            var store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JObject>();
                var dizlist = result["dnamelist"].ToObject<JArray>();
                for (var i = 0; i < dizlist.Count; i++)
                {
                    var discuz = new QQDiscuz();
                    var dizjson = dizlist[i].ToObject<JObject>();
                    discuz.Did = dizjson["did"].ToObject<long>();
                    discuz.Name = dizjson["name"].ToString();
                    store.AddDiscuz(discuz);
                }
                NotifyActionEvent(QQActionEventType.EvtOK, store.GetDiscuzList());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }
        }
    }

}
