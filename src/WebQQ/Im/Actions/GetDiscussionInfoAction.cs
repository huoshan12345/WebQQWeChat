using System.Linq;
using AutoMapper;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetDiscussionInfoAction : WebQQInfoAction
    {
        private readonly QQDiscussion _discussion;
        public GetDiscussionInfoAction(IQQContext context, QQDiscussion discussion, ActionEventListener listener = null) : base(context, listener)
        {
            _discussion = discussion;
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("clientid", Session.ClientId);
            req.AddData("psessionid", Session.SessionId);
            req.AddData("vfwebqq", Session.Vfwebqq);
            req.AddData("t", Timestamp);
            req.AddData("did", _discussion.Did);
            req.Referrer = ApiUrls.Referrer;
        }

        protected override void HandleResult(JToken json)
        {
            /*
                {
                    "result": {
                        "info": {
                            "did": 3774084691,
                            "discu_name": "Test",
                            "mem_list": [
                                {
                                    "mem_uin": 89009143, // uin
                                    "ruin": 89009143     // qq号
                                },
                            ]
                        },
                        "mem_info": [
                            {
                                "nick": "月光双刀",
                                "uin": 89009143
                            },
                        ],
                        "mem_status": [
                            {
                                "client_type": 7,
                                "status": "online",
                                "uin": 89009143
                            },
                        ]
                    },
                    "retcode": 0
                }             
             */

            var result = json["result"];

            // 成员列表
            var members = result["info"]["mem_list"].ToObject<DiscussionMember[]>().Distinct(m => m.Uin).ToDictionary(m => m.Uin, m => m);

            // 昵称
            var mInfo = result["mem_info"].ToJArray();
            foreach (var info in mInfo)
            {
                var key = info["uin"].ToLong();
                members.GetAndDo(key, m => m.Nick = info["nick"].ToString());
            }

            // 成员状态
            var mStatus = result["mem_status"].ToObject<DiscussionMemberStatus[]>();
            foreach (var status in mStatus)
            {
                members.GetAndDo(status.Uin, member => Mapper.Map(status, member));
            }

            _discussion.Members.ReplaceBy(members);
        }
    }
}
