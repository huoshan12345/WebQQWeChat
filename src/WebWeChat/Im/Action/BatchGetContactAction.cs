using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using FxUtility.Extensions;
using HttpAction.Core;
using HttpAction.Event;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 用于获取群成员
    /// </summary>
    [Description("获取群成员")]
    public class BatchGetContactAction : WeChatAction
    {
        public BatchGetContactAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.BatchGetContact, Session.BaseUrl, Session.PassTicket, Timestamp);
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

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
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
