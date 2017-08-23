using System.ComponentModel;
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

        protected override HttpRequestItem BuildRequest()
        {
            var req =  new HttpRequestItem(HttpMethodType.Post, string.Format(ApiUrls.GetQRCode, Session.Uuid));
            req.AddQueryValue("t", "webwx");
            req.AddQueryValue("_", Session.Seq++);
            req.ResultType = HttpResultType.Byte;
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
        {
            // return NotifyOkEventAsync(Image.FromStream(responseItem.ResponseStream));
            return NotifyOkEventAsync(ImageSharp.Image.Load(responseItem.ResponseBytes));
        }
    }
}
