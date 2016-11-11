using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetFriendQQNumberAction : WebQQInfoAction
    {
        private readonly QQFriend _friend;

        public GetFriendQQNumberAction(IQQContext context, QQFriend friend, ActionEventListener listener = null)
            : base(context, listener)
        {
            _friend = friend;
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddQueryValue("tuin", _friend.Uin);
            req.AddQueryValue("type", 1);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.ReferrerS;
        }

        protected override void HandleResult(JToken json)
        {
            /*
                 {
                    "retcode": 0,
                    "result": {
                        "uiuin": "", // 总是为空，所以忽略了
                        "account": 510942549,
                        "uin": 3943520589
                    }
                }
            */
            _friend.QQNumber = json["result"]["account"].ToLong();
        }
    }
}
