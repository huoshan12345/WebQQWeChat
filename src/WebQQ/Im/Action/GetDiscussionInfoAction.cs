using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Core;
using WebQQ.Util;
using FxUtility.Extensions;
using WebQQ.Im.Bean;

namespace WebQQ.Im.Action
{
    public class GetDiscussionInfoAction : QQAction
    {
        private readonly QQDiscussion _discussion;
        public GetDiscussionInfoAction(IQQContext context, QQDiscussion discussion, ActionEventListener listener = null) : base(context, listener)
        {
            _discussion = discussion;
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetDiscussionInfo);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", Session.SessionId);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            req.AddQueryValue("did", _discussion.Did);
            req.Referrer = ApiUrls.Referrer;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
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
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
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

                _discussion.Members.Clear();
                _discussion.Members.AddOrUpdateRange(members);

                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
