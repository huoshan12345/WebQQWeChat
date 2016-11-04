using System.ComponentModel;
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
    /// 开启状态通知
    /// </summary>
    [Description("开启状态通知")]
    public class StatusNotifyAction : WeChatAction
    {
        public StatusNotifyAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.StatusNotify, Session.BaseUrl, Session.PassTicket);
            var obj = new
            {
                Session.BaseRequest,
                Code = 3,
                FromUserName = Session.User["UserName"],
                ToUserName = Session.User["UserName"],
                ClientMsgId = Timestamp
            };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                RawData = JsonConvert.SerializeObject(obj),
                ContentType = HttpConstants.JsonContentType,
            };
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
                    "MsgID": "5895072760632094896"
                }
            */
            var str = responseItem.ResponseString;
            if (!str.IsNullOrEmpty())
            {
                var json = JObject.Parse(str);
                if (json["BaseResponse"]["Ret"].ToString() == "0")
                {
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
