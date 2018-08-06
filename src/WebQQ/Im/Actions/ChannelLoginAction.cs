using FclEx.Extensions;
using HttpAction;
using HttpAction.Core;
using FclEx.Http.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class ChannelLoginAction : WebQQInfoAction
    {
        public ChannelLoginAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Form;

        protected override void ModifyRequest(HttpRequestItem req)
        {
            var json = new JObject
            {
                {"status", QQStatusType.Online.ToString().ToLower()},
                {"ptwebqq", Session.Ptwebqq},
                {"clientid", Session.ClientId},
                {"psessionid", ""}
            };
            req.AddData("r", json.ToSimpleString());
            req.Referrer = ApiUrls.Referrer;
        }

        protected override void HandleResult(JToken json)
        {
            var ret = json["result"];
            Session.User.Uin = ret["uin"].ToLong();
            Session.User.Status = ret["status"].ToEnum(QQStatusType.Online);
            Session.SessionId = ret["psessionid"].ToString();
            Session.Index = ret["index"].ToInt();
            Session.Port = ret["port"].ToInt();
            // Session.Vfwebqq = ret["vfwebqq"].ToString();
        }
    }
}
