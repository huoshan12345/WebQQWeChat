using System;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetVFWebqq : AbstractHttpAction
    {
        public GetVFWebqq(IQQContext context, QQActionEventHandler listener) : base(context, listener)
        {
        }

        public override QQHttpRequest BuildRequest()
        {
            var httpService = Context.GetSerivce<HttpService>(QQServiceType.HTTP);
            var session = Context.Session;
            var request = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_VFWEBQQ);
            request.AddGetValue("ptwebqq", httpService.GetCookie("ptwebqq", QQConstants.URL_CHANNEL_LOGIN).Value);
            request.AddGetValue("clientid", session.ClientId);
            request.AddGetValue("psessionid", "");
            request.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            return request;
        }


        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.GetResponseString();
            var json = JObject.Parse(str);
            if (json["retcode"].ToString() == "0")
            {
                var ret = json["result"].ToObject<JObject>();
                Context.Session.Vfwebqq = ret["vfwebqq"].ToString();
                NotifyActionEvent(QQActionEventType.EvtOK, null);
            }
            else
            {
                // NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.INVALID_RESPONSE));    //TODO ..
                throw new QQException(QQErrorCode.INVALID_RESPONSE, str);
            }
        }
    }
}
