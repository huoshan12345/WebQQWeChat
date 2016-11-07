using System;
using System.Drawing;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;

namespace WebQQ.Im.Action
{
    public class GetQRCodeAction : QQAction
    {
        public GetQRCodeAction(IQQContext context, ActionEventListener listener = null) : base(context, listener) { }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Get, QQConstants.URL_GET_QRCODE);
            req.AddQueryValue("appid", QQConstants.APPID);
            req.AddQueryValue("e", "0");
            req.AddQueryValue("l", "M");
            req.AddQueryValue("s", "5");
            req.AddQueryValue("d", "72");
            req.AddQueryValue("v", "4");
            req.AddQueryValue("t", new Random().NextDouble());
            //req.AddRefer(QQConstants.URL_LOGIN_PAGE);
            //req.AddHeader(HttpConstants.SetCookie, "qrsig=dG0lVGD8IhpDl1cMsy4qgghLk24rOwSK9YVq2YlWAjBzJ69tIE-9sFkMttULkrww; PATH=/; DOMAIN=ptlogin2.qq.com;");
            req.ResultType = HttpResultType.Stream;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK, Image.FromStream(response.ResponseStream));
        }
    }
}
