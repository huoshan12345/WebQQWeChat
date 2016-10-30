using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using Utility.Extensions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 用于获取群成员
    /// </summary>
    public class BatchGetContactAction : WebWeChatAction
    {
        public BatchGetContactAction(ActionEventListener listener = null)
            : base(listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.BatchGetContact, Session.BaseUrl);
            var obj = new
            {
                Session.BaseRequest,
                Count = Store.GroupCount,
                List = Store.Groups.Select(m => new { m.UserName, EncryChatRoomId = "" })
            };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                ContentType = HttpConstants.JsonContentType,
                RawData = JsonConvert.SerializeObject(obj)
            };
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            if (!str.IsNullOrEmpty())
            {
                var json = JObject.Parse(str);
                if (json["BaseResponse"]["Ret"].ToString() == "0")
                {
                    var list = json["ContactList"].ToObject<List<ContactMember>>();
                    foreach (var item in list)
                    {
                        Store.ContactMemberDic[item.UserName] = item;
                    }
                    NotifyActionEvent(ActionEventType.EvtOK);
                    return;
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
