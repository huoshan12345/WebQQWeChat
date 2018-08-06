using FclEx.Extensions;
using HttpAction.Core;
using FclEx.Http.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
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
            req.AddData("tuin", _friend.Uin);
            req.AddData("type", 1);
            req.AddData("vfwebqq", Session.Vfwebqq);
            req.AddData("t", Timestamp);
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
