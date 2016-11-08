using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FxUtility.Helpers;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class ChannelLoginAction : QQAction
    {
        public ChannelLoginAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.ChannelLogin);
            var json = new JObject
            {
                {"status", QQStatusType.Online.ToString().ToLower()},
                {"ptwebqq", Session.Ptwebqq},
                {"clientid", Session.ClientId},
                {"psessionid", ""}
            }.ToString(Formatting.None);
            req.AddQueryValue("r", json);
            req.AddHeader(HttpConstants.Referrer, ApiUrls.Referrer);
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJObject();
            if (json["retcode"].ToString() == "0")
            {
                var ret = json["result"].ToJObject();
                Account.User.Uin = ret["uin"].ToObject<long>();
                Account.User.QQ = ret["uin"].ToObject<long>();
                Session.SessionId = ret["psessionid"].ToString();
                Account.User.Status = ret["status"].ToString().ToEnum(QQStatusType.Online);
                Session.State = SessionState.Online;
                Session.Index = ret["index"].ToInt();
                Session.Port = ret["port"].ToInt();
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
