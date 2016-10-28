using System.Drawing;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class GetQRCodeAction : WebWeChatAction
    {
        public GetQRCodeAction(IWeChatContext context, ActionEventListener listener) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req =  new HttpRequestItem(HttpMethodType.Post, string.Format(ApiUrls.GetQRCode, Session.Uuid));
            req.AddQueryValue("t", "webwx");
            req.AddQueryValue("_", Timestamp);
            req.ResultType = ResponseResultType.Stream;
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            NotifyActionEvent(ActionEventType.EvtOK, Image.FromStream(responseItem.ResponseStream));
        }
    }
}
