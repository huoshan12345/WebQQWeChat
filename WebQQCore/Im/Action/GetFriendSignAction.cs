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
    /// <para>个人签名</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetFriendSignAction : AbstractHttpAction
    {

        private readonly QQUser _buddy;

        public GetFriendSignAction(IQQContext context, QQActionEventHandler listener,QQUser buddy)
            : base(context, listener)
        {
            this._buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;

            var req = CreateHttpRequest(HttpConstants.Get,
                    QQConstants.URL_GET_USER_SIGN);
            req.AddGetValue("tuin", _buddy.Uin);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JArray>();
                var obj = result[0].ToObject<JObject>();
                _buddy.Sign = obj["lnick"].ToString();
            }

            NotifyActionEvent(QQActionEventType.EVT_OK, _buddy);
        }

    }

}
