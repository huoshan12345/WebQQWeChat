using System;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取群图片 key and sig</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetCustomFaceSigAction : AbstractHttpAction
    {
        public GetCustomFaceSigAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_CUSTOM_FACE_SIG);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis() / 1000 + "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            QQSession session = Context.Session;

            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                JObject obj = json["result"].ToObject<JObject>();
                session.CfaceKey = obj["gface_key"].ToString();
                session.CfaceSig = obj["gface_sig"].ToString();
                NotifyActionEvent(QQActionEventType.EVT_OK, session);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                    new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }
        }

    }

}
