using System;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetQRCodeAction : WebQQAction
    {
        private const string AppId = "501004106";

        public GetQRCodeAction(IQQContext context, ActionEventListener listener = null) : base(context, listener) { }

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Get;

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("appid", AppId);
            req.AddData("e", "0");
            req.AddData("l", "M");
            req.AddData("s", "5");
            req.AddData("d", "72");
            req.AddData("v", "4");
            req.AddData("t", new Random().NextDouble());
            req.ResultType = HttpResultType.Byte;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            return NotifyOkEventAsync(ImageSharp.Image.Load(response.ResponseBytes));
        }
    }
}
