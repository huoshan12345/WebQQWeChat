using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    /// <summary>
    /// 获取好友个性签名
    /// </summary>
    public class GetFriendLongNickAction : QQAction
    {
        private readonly Friend _friend;

        public GetFriendLongNickAction(IQQContext context, Friend friend, ActionEventListener listener = null) : base(context, listener)
        {
            _friend = friend;
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetFriendLongNick);
            req.AddQueryValue("tuin", _friend.Uin);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            /*
                {
                    "retcode": 0,
                    "result": [
                        {
                            "uin": 984536900,
                            "lnick": ""
                        }
                    ]
                }
             */
            var json = response.ResponseString.ToJObject();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"];
                var uin = result["uin"].ToLong();
                Store.FriendDic.GetAndDo(uin, friend => friend.LongNick = result["lnick"].ToString());
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
