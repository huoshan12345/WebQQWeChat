using AutoMapper;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetFriendInfoAction : WebQQInfoAction
    {
        private readonly QQFriend _friend;

        public GetFriendInfoAction(IQQContext context, QQFriend friend, ActionEventListener listener = null) 
            : base(context, listener)
        {
            _friend = friend;
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("tuin", _friend.Uin);
            req.AddData("clientid", Session.ClientId);
            req.AddData("psessionid", Session.SessionId);
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
                        "face": 150,
                        "birthday": {
                            "month": 1,
                            "year": 1989,
                            "day": 4
                        },
                        "occupation": "计算机业",
                        "phone": "-",
                        "allow": 1,
                        "college": "回中",
                        "uin": 3943520589,
                        "constel": 12,
                        "blood": 2,
                        "homepage": "",
                        "stat": 20,
                        "vip_info": 0,
                        "country": "中国",
                        "city": "",
                        "personal": "",
                        "nick": "月光酸甜",
                        "shengxiao": 5,
                        "email": "510942549@qq.com",
                        "province": "北京",
                        "gender": "female",
                        "mobile": ""
                    }
                }
            */
            var info =  json["result"].ToObject<FriendInfo>();
            Mapper.Map(info, _friend);
        }
    }
}
