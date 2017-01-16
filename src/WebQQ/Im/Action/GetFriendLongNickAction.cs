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
using FclEx.Extensions;
using WebQQ.Im.Bean.Friend;

namespace WebQQ.Im.Action
{
    /// <summary>
    /// 获取好友个性签名
    /// </summary>
    public class GetFriendLongNickAction : WebQQInfoAction
    {
        private readonly QQFriend _friend;

        public GetFriendLongNickAction(IQQContext context, QQFriend friend, ActionEventListener listener = null) : base(context, listener)
        {
            _friend = friend;
        }


        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddQueryValue("tuin", _friend.Uin);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.ReferrerS;
        }

        protected override void HandleResult(JToken json)
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
            _friend.LongNick = json["result"][0]["lnick"].ToString();
        }
    }
}
