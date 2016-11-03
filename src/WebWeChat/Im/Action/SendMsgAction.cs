using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Util;

namespace WebWeChat.Im.Action
{
    [Description("发送消息")]
    public class SendMsgAction : WeChatAction
    {
        private readonly MessageSent _msg;

        public SendMsgAction(IWeChatContext context, MessageSent msg, ActionEventListener listener = null) : base(context, listener)
        {
            _msg = msg;
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.SendMsg, Session.BaseUrl);
            var obj = new
            {
                Session.BaseRequest,
                Msg = _msg
            };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                RawData = obj.ToJson(),
                ContentType = HttpConstants.JsonContentType
            };
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJsonObj();
            if (json["BaseResponse"]["Ret"].ToString() == "0")
            {
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new WeChatException(WeChatErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
