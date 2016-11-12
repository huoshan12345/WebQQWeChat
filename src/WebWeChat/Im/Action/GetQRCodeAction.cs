using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 获取二维码
    /// </summary>
    [Description("获取二维码")]
    public class GetQRCodeAction : WebWeChatAction
    {
        public GetQRCodeAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req =  new HttpRequestItem(HttpMethodType.Post, string.Format(ApiUrls.GetQRCode, Session.Uuid));
            req.AddQueryValue("t", "webwx");
            req.AddQueryValue("_", Session.Seq++);
            req.ResultType = HttpResultType.Stream;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK, Image.FromStream(responseItem.ResponseStream));
        }
    }
}
