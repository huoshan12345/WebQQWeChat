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
    /// <para>获取用户等级</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetUserLevelAction : AbstractHttpAction
    {
        private QQUser user;
 
        public GetUserLevelAction(IQQContext context, QQActionEventHandler listener, QQUser user)
            : base(context, listener)
        {

            this.user = user;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JObject>();
                var level = user.Level;
                level.Level = result["level"].ToObject<int>();
                level.Days = result["days"].ToObject<int>();
                level.Hours = result["hours"].ToObject<int>();
                level.RemainDays = result["remainDays"].ToObject<int>();
                NotifyActionEvent(QQActionEventType.EvtOK, user);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError,
                        new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_USER_LEVEL);
            var session = Context.Session;
            req.AddGetValue("tuin", user.Uin);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            return req;
        }
    }

}
