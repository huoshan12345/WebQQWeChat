using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FxUtility.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetOnlineFriendsAction : QQAction
    {
        public GetOnlineFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetOnlineFriends);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", Session.SessionId);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.Referrer;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            /*
                {
                    "result": [
                        {
                            "client_type": 1,
                            "status": "online",
                            "uin": 3017767504
                        }
                    ],
                    "retcode": 0
                }
            */
            var json = response.ResponseString.ToJObject();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToJArray();
                foreach (var token in result)
                {
                    var uin = token["uin"].ToLong();
                    var buddy = Store.GetFriendByUin(uin);
                    if (buddy != null)
                    {
                        buddy.Status = token["status"].ToString().ToEnum<QQStatusType>();
                        buddy.ClientType = token["client_type"].ToInt();
                    }
                }
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
