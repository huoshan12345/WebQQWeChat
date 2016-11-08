using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetFriendsAction : QQAction
    {
        public GetFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var json = new JObject
            {
                {"h", "hello"},
                {"vfwebqq", Session.Vfwebqq},
                {"hash", QQEncryptor.GetHash(Account.User.Uin.ToString(), Session.Ptwebqq)}
            }.ToString(Formatting.None);
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.GetFriends);
            req.AddQueryValue("r", json);
            req.Referrer = ApiUrls.ReferrerS;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJsonObj();
            if (json["retcode"].ToString() == "0")
            {
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
