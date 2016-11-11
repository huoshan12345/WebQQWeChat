using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetDiscussionListAction : QQAction
    {
        public GetDiscussionListAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetDiscussionList);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", Session.SessionId);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.ReferrerS;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            /*
                {
                    "retcode": 0,
                    "result": {
                        "dnamelist": [
                            {
                                "name": "月光双刀、Test、月光借口、月光双",
                                "did": 522140442
                            }
                        ]
                    }
                }             
             */
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"];
                var list = result["dnamelist"].ToObject<List<QQDiscussion>>();
                list.ForEach(Store.AddDiscussion);
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
