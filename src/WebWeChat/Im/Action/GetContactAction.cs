using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utility.Extensions;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class GetContactAction : WebWeChatAction
    {
        public GetContactAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.GetContact, Session.BaseUrl);
            var obj = new { Session.BaseRequest };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                RawData = JsonConvert.SerializeObject(obj),
                ContentType = HttpConstants.JsonContentType
            };
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            /*
                {
                    "BaseResponse": {
                        "Ret": 0,
                        "ErrMsg": ""
                    },
                    "MemberCount": 334,
                    "MemberList": [
                        {
                            "Uin": 0,
                            "UserName": xxx,
                            "NickName": "Urinx",
                            "HeadImgUrl": xxx,
                            "ContactFlag": 3,
                            "MemberCount": 0,
                            "MemberList": [],
                            "RemarkName": "",
                            "HideInputBarFlag": 0,
                            "Sex": 0,
                            "Signature": "我是二蛋",
                            "VerifyFlag": 8,
                            "OwnerUin": 0,
                            "PYInitial": "URINX",
                            "PYQuanPin": "Urinx",
                            "RemarkPYInitial": "",
                            "RemarkPYQuanPin": "",
                            "StarFriend": 0,
                            "AppAccountFlag": 0,
                            "Statues": 0,
                            "AttrStatus": 0,
                            "Province": "",
                            "City": "",
                            "Alias": "Urinxs",
                            "SnsFlag": 0,
                            "UniFriend": 0,
                            "DisplayName": "",
                            "ChatRoomId": 0,
                            "KeyWord": "gh_",
                            "EncryChatRoomId": ""
                        },
                        ...
                    ],
                    "Seq": 0
                }
            */
            var str = responseItem.ResponseString;
            if (!str.IsNullOrEmpty())
            {
                var json = JObject.Parse(str);
                if (json["BaseResponse"]["Ret"].ToString() == "0")
                {
                    Store.MemberCount = json["MemberCount"].ToObject<int>();
                    var list = json["MemberList"].ToObject<List<Member>>();
                    Store.MemberDic = list.ToDictionary(m => m.UserName);

                    NotifyActionEvent(ActionEventType.EvtOK);
                    return;
                }
                else
                {
                    throw new WeChatException(WeChatErrorCode.ResponseError, json["BaseResponse"]["ErrMsg"].ToString());
                }

            }
            throw new WeChatException(WeChatErrorCode.ResponseError);
        }

    }
}
