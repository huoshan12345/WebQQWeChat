using System.Threading.Tasks;
using AutoMapper;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetOnlineFriendsAction : WebQQInfoAction
    {
        public GetOnlineFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("vfwebqq", Session.Vfwebqq);
            req.AddData("clientid", Session.ClientId);
            req.AddData("psessionid", Session.SessionId);
            req.AddData("t", Timestamp);
            req.Referrer = ApiUrls.Referrer;
        }

        protected override void HandleResult(JToken json)
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

            var result = json["result"].ToObject<FriendOnlineInfo[]>();
            foreach (var info in result)
            {
                Store.FriendDic.GetAndDo(info.Uin, friend => Mapper.Map(info, friend));
            }
        }
    }
}
