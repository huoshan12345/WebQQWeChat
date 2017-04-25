using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebWeChat.Im.Core;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 开启状态通知
    /// </summary>
    [Description("开启状态通知")]
    public class StatusNotifyAction : WebWeChatAction
    {
        public StatusNotifyAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        protected override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.StatusNotify, Session.BaseUrl, Session.PassTicket);
            var obj = new
            {
                Session.BaseRequest,
                Code = 3,
                FromUserName = Session.UserToken["UserName"],
                ToUserName = Session.UserToken["UserName"],
                ClientMsgId = Timestamp
            };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                RawData = JsonConvert.SerializeObject(obj),
                ContentType = HttpConstants.JsonContentType,
            };
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
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
                    return NotifyOkEventAsync();
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
