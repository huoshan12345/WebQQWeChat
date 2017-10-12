using System;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Extensions;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetQRCodeAction : WebQQAction
    {
        private const string AppId = "501004106";

        public GetQRCodeAction(IQQContext context, ActionEventListener listener = null) : base(context, listener) { }

        protected override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Get, ApiUrls.GetQRCode);
            req.AddQueryValue("appid", AppId);
            req.AddQueryValue("e", "0");
            req.AddQueryValue("l", "M");
            req.AddQueryValue("s", "5");
            req.AddQueryValue("d", "72");
            req.AddQueryValue("v", "4");
            req.AddQueryValue("t", new Random().NextDouble());
            req.ResultType = HttpResultType.Byte;
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            // return NotifyOkEventAsync(Image.FromStream(response.ResponseStream));

            return NotifyOkEventAsync(ImageSharp.Image.Load(response.ResponseBytes));
        }
    }
}
