using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Core;
using FxUtility.Extensions;
using HttpAction.Core;
using HttpAction.Event;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 微信初始化
    /// 获取初始化信息（账号头像信息、聊天好友、阅读等）
    /// </summary>
    [Description("微信初始化")]
    public class WebwxInitAction : WebWeChatAction
    {
        public WebwxInitAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.WebwxInit, Session.BaseUrl, Session.PassTicket, Session.Skey, Timestamp);
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

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
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
            /*
                本次ContactList里只有10个好友or群组，应该是最近的10个活跃对象（个数不是固定的）；
                另外，可以通过UserName来区分好友or群组，一个”@”为好友，两个”@”为群组。
                MPSubscribeMsg为公众号推送的阅读文章
                User其实就是自己账号信息（用在顶部的头像） 
             */
            var str = responseItem.ResponseString;
            if (!str.IsNullOrEmpty())
            {
                var json = JObject.Parse(str);
                if (json["BaseResponse"]["Ret"].ToString() == "0")
                {
                    Session.SyncKey = json["SyncKey"];
                    // Session.SyncKeyStr = Session.SyncKey["List"].ToArray().Select(m => $"{m["Key"]}_{m["Val"]}").JoinWith("|");
                    Session.UserToken = json["User"];
                    return NotifyActionEventAsync(ActionEventType.EvtOK);
                }
                else
                {
                    throw new WeChatException(WeChatErrorCode.ResponseError, json["BaseResponse"]["ErrMsg"].ToString());
                }

            }
            throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }
    }
}
