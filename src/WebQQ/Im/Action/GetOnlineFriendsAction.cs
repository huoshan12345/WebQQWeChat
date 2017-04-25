using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetOnlineFriendsAction : WebQQAction
    {
        public GetOnlineFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetOnlineFriends);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", Session.SessionId);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.Referrer;
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
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
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<FriendOnlineInfo[]>();
                foreach (var info in result)
                {
                    Store.FriendDic.GetAndDo(info.Uin, friend => Mapper.Map(info, friend));
                }
                return NotifyOkEventAsync();
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
