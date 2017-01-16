using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using FclEx.Extensions;
using FclEx.Extesions;

namespace WebQQ.Im.Action
{
    public class ChannelLoginAction : WebQQInfoAction
    {
        public ChannelLoginAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.Method = HttpMethodType.Post;
            var json = new JObject
            {
                {"status", QQStatusType.Online.ToLowerString()},
                {"ptwebqq", Session.Ptwebqq},
                {"clientid", Session.ClientId},
                {"psessionid", ""}
            };
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = ApiUrls.Referrer;
        }

        protected override void HandleResult(JToken json)
        {
            var ret = json["result"];
            Session.User.Uin = ret["uin"].ToLong();
            Session.User.Status = ret["status"].ToEnum(QQStatusType.Online);
            Session.SessionId = ret["psessionid"].ToString();
            Session.State = SessionState.Online;
            Session.Index = ret["index"].ToInt();
            Session.Port = ret["port"].ToInt();
        }
    }
}
