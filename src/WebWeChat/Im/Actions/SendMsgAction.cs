using System.ComponentModel;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Actions
{
    [Description("发送消息")]
    public class SendMsgAction : WebWeChatAction
    {
        private readonly MessageSent _msg;

        public SendMsgAction(IWeChatContext context, MessageSent msg, ActionEventListener listener = null) : base(context, listener)
        {
            _msg = msg;
        }

        protected override HttpReq BuildRequest()
        {
            var url = string.Format(ApiUrls.SendMsg, Session.BaseUrl);
            var obj = new
            {
                Session.BaseRequest,
                Msg = _msg
            };
            var req = new HttpReq(HttpMethodType.Post, url)
            {
                StringData = obj.ToJson(),
                ContentType = HttpConstants.JsonContentType
            };
            return req;
        }

        protected override ValueTask<ActionEvent> HandleResponse(HttpRes response)
        {
            var json = response.ResponseString.ToJToken();
            if (json["BaseResponse"]["Ret"].ToString() == "0")
            {
                return NotifyOkEventAsync();
            }
            else
            {
                throw new WeChatException(WeChatErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
