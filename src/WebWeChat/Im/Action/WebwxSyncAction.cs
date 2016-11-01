using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Utility.Extensions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Util;

namespace WebWeChat.Im.Action
{
    public class WebwxSyncAction:WeChatAction
    {
        public WebwxSyncAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = string.Format(ApiUrls.WebwxSync, Session.BaseUrl);
            var obj = new
            {
                Session.BaseRequest,
                Session.SyncKey,
                rr = ~Timestamp // 注意是按位取反
            };
            var req = new HttpRequestItem(HttpMethodType.Post, url)
            {
                // 此处需要将key都变成小写，否则提交会失败
                RawData = obj.ToJson(),
                ContentType = HttpConstants.JsonContentType,
            };
            return req;
        }

        public override ActionEvent HandleResponse(HttpResponseItem response)
        {
            var str = response.ResponseString;
            var json = JObject.Parse(str);
            if (json["BaseResponse"]["Ret"].ToString() == "0")
            {
                Session.SyncKey = json["SyncKey"];
                var list = json["AddMsgList"].ToObject<List<Message>>();
                return NotifyActionEvent(ActionEventType.EvtOK, list);
            }
            throw WeChatException.CreateException(WeChatErrorCode.ResponseError);

        }
    }
}
