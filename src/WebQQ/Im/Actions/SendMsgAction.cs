using System;
using System.Threading;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class SendMsgAction : WebQQAction
    {
        private readonly Message _msg;
        private static long _msgId = 81690000;

        public SendMsgAction(IQQContext context, Message msg, ActionEventListener listener = null) : base(context, listener)
        {
            _msg = msg;
        }

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Form;

        protected override HttpRequestItem BuildRequest()
        {
            HttpRequestItem req = null;
            var json = new JObject()
            {
                ["content"] = _msg.PackContentList(),
                ["msg_id"] = Interlocked.Increment(ref _msgId),
                ["clientid"] = Session.ClientId,
                ["psessionid"] = Session.SessionId,
            };

            json.Add("face", 252); // 不知道有什么卵用

            switch (_msg)
            {
                case FriendMessage fMsg:
                    /*
                        {
                           "to": 3269846909,
                           "content": "[\"嗯\",[\"font\",{\"name\":\"宋体\",\"size\":10,\"style\":[0,0,0],\"color\":\"000000\"}]]",
                           "face": 252,
                           "clientid": 53999199,
                           "msg_id": 22700002,
                           "psessionid": "8368046764001d636f6e6e7365727665725f77656271714031302e3133332e34312e383400001ad00000066b026e040015808a206d0000000a406172314338344a69526d0000002859185d94e66218548d1ecb1a12513c86126b3afb97a3c2955b1070324790733ddb059ab166de6857"
                       }                 
                    */
                    json.Add("to", fMsg.Friend.Uin);
                    req = HttpRequestItem.CreateFormRequest(ApiUrls.SendFriendMsg);
                    break;

                case GroupMessage gMsg:
                    /*
                        {
                            "group_uin": 95198668,
                            "content": "[\"有人没\",[\"font\",{\"name\":\"宋体\",\"size\":10,\"style\":[0,0,0],\"color\":\"000000\"}]]",
                            "face": 252,
                            "clientid": 53999199,
                            "msg_id": 22700001,
                            "psessionid": "8368046764001d636f6e6e7365727665725f77656271714031302e3133332e34312e383400001ad00000066b026e040015808a206d0000000a406172314338344a69526d0000002859185d94e66218548d1ecb1a12513c86126b3afb97a3c2955b1070324790733ddb059ab166de6857"
                        }               
                    */
                    req = HttpRequestItem.CreateFormRequest(ApiUrls.SendGroupMsg);
                    json.Add("group_uin", gMsg.Group.Gid);
                    break;

                case DiscussionMessage dMsg:
                    /*
                        {
                            "did": 800220077,
                            "content": "[\"asdf\",[\"font\",{\"name\":\"宋体\",\"size\":10,\"style\":[0,0,0],\"color\":\"000000\"}]]",
                            "face": 252,
                            "clientid": 53999199,
                            "msg_id": 22700003,
                            "psessionid": "8368046764001d636f6e6e7365727665725f77656271714031302e3133332e34312e383400001ad00000066b026e040015808a206d0000000a406172314338344a69526d0000002859185d94e66218548d1ecb1a12513c86126b3afb97a3c2955b1070324790733ddb059ab166de6857"
                        }                     
                     */

                    req = HttpRequestItem.CreateFormRequest(ApiUrls.SendDiscussionMsg);
                    json.Add("did", dMsg.Discussion.Did);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(_msg));
            }
            req.Referrer = "https://d1.web2.qq.com/cfproxy.html?v=20151105001&callback=1";
            req.AddData("r", json.ToJson());
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            var retcode = json["retcode"]?.ToString();
            return retcode.IsNull() ?
                NotifyOkEventAsync() :
                NotifyErrorAsync(response.ResponseString);
        }
    }
}
