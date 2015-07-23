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

        private QQUser buddy;

        public GetFriendSignAction(QQContext context, QQActionEventHandler listener,QQUser buddy)
            : base(context, listener)
        {
            this.buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_GET_USER_SIGN);
            req.AddGetValue("tuin", buddy.Uin + "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                JArray result = json["result"].ToObject<JArray>();
                JObject obj = result[0].ToObject<JObject>();
                buddy.Sign = obj["lnick"].ToString();
            }

            NotifyActionEvent(QQActionEventType.EVT_OK, buddy);
        }

    }

}
