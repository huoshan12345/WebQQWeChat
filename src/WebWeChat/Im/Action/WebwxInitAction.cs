using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Core;
using Utility.Extensions;

namespace WebWeChat.Im.Action
{
    public class WebwxInitAction : WebWeChatAction
    {
        public WebwxInitAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.WebwxInit, Session.BaseUrl);
            var req = new HttpRequestItem(HttpMethodType.Post, url);
            var obj = new { Session.BaseRequest };
            /*
                {
                    "BaseRequest": {
                        "DeviceId": "e650946746417762",
                        "Skey": "@crypt_c498484a_1d7a344b3232380eb1aa33c16690399a",
                        "Sid": "PhHAnhCRcFDCA219",
                        "Uin": "463678295"
                    }
                }             
            */
            req.RawData = JsonConvert.SerializeObject(obj);
            req.ContentType = HttpConstants.JsonContentType;
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
                    "Count": 11,
                    "ContactList": [...],
                    "SyncKey": {
                        "Count": 4,
                        "List": [
                            {
                                "Key": 1,
                                "Val": 635705559
                            },
                            ...
                        ]
                    },
                    "User": {
                        "Uin": xxx,
                        "UserName": xxx,
                        "NickName": xxx,
                        "HeadImgUrl": xxx,
                        "RemarkName": "",
                        "PYInitial": "",
                        "PYQuanPin": "",
                        "RemarkPYInitial": "",
                        "RemarkPYQuanPin": "",
                        "HideInputBarFlag": 0,
                        "StarFriend": 0,
                        "Sex": 1,
                        "Signature": "Apt-get install B",
                        "AppAccountFlag": 0,
                        "VerifyFlag": 0,
                        "ContactFlag": 0,
                        "WebWxPluginSwitch": 0,
                        "HeadImgFlag": 1,
                        "SnsFlag": 17
                    },
                    "ChatSet": xxx,
                    "SKey": xxx,
                    "ClientVersion": 369297683,
                    "SystemTime": 1453124908,
                    "GrayScale": 1,
                    "InviteStartCount": 40,
                    "MPSubscribeMsgCount": 2,
                    "MPSubscribeMsgList": [...],
                    "ClickReportInterval": 600000
                } 
             */
            var str = responseItem.ResponseString;
            if (!str.IsNullOrEmpty())
            {
                var json = JObject.Parse(str);
                if (json["BaseResponse"]["Ret"].ToString() == "0")
                {
                    Session.SyncKey = json["SyncKey"];
                    Session.User = json["User"];
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
