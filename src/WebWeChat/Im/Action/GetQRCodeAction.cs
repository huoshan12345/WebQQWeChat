using System.Drawing;
using System.Threading.Tasks;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class GetQRCodeAction : WeChatAction
    {
        /// <summary>
        /// 显示二维码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="listener"></param>
        public GetQRCodeAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req =  new HttpRequestItem(HttpMethodType.Post, string.Format(ApiUrls.GetQRCode, Session.Uuid));
            req.AddQueryValue("t", "webwx");
            req.AddQueryValue("_", Timestamp);
            req.ResultType = HttpResultType.Stream;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK, Image.FromStream(responseItem.ResponseStream));
        }
    }
}
