using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        const string url = "http://s.web2.qq.com/api/getvfwebqq";

        /**
         * <p>Constructor for AbstractHttpAction.</p>
         *
         * @param context  a {@link IQQContext} object.
         * @param listener a {@link QQActionListener} object.
         */
        public GetVFWebqq(IQQContext context, QQActionEventHandler listener) : base(context, listener)
        {
            ;
        }


        public override QQHttpRequest BuildRequest()
        {
            HttpService httpService = Context.GetSerivce<HttpService>(QQServiceType.HTTP);
            QQSession session = Context.Session;

            QQHttpRequest request = CreateHttpRequest("GET", url);
            request.AddGetValue("ptwebqq", httpService.GetCookie("ptwebqq", QQConstants.URL_CHANNEL_LOGIN).Value);
            request.AddGetValue("clientid", session.ClientId);
            request.AddGetValue("psessionid", "");
            request.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            return request;
        }


        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            QQSession session = Context.Session;

            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                JObject ret = json["result"].ToObject<JObject>();
                session.Vfwebqq = ret["vfwebqq"].ToString();
                NotifyActionEvent(QQActionEventType.EVT_OK, null);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.INVALID_RESPONSE));    //TODO ..
            }
        }
    }
}
