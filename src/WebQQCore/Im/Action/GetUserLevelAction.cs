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
        private readonly QQUser _user;
 
        public GetUserLevelAction(IQQContext context, QQActionListener listener, QQUser user)
            : base(context, listener)
        {

            _user = user;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.ResponseString;
            var json = JObject.Parse(str);
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JObject>();
                var level = _user.LevelInfo;
                level.Level = result["level"].ToObject<int>();
                level.Days = result["days"].ToObject<int>();
                level.Hours = result["hours"].ToObject<int>();
                level.RemainDays = result["remainDays"].ToObject<int>();
                NotifyActionEvent(QQActionEventType.EvtOK, _user);
            }
            else
            {
                throw new QQException(QQErrorCode.UnexpectedResponse, str);
            }
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_USER_LEVEL);
            var session = Context.Session;
            req.AddGetValue("tuin", _user.Uin);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            return req;
        }
    }

}
