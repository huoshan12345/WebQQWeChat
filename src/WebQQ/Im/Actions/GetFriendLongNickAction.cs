using HttpAction.Core;
using FclEx.Http.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
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


        protected override void ModifyRequest(HttpReq req)
        {
            req.AddData("tuin", _friend.Uin);
            req.AddData("vfwebqq", Session.Vfwebqq);
            req.AddData("t", Timestamp);
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
