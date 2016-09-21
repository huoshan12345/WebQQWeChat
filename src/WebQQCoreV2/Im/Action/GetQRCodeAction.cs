using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetQRCodeAction : AbstractWebQQAction
    {
        public GetQRCodeAction(IQQContext context, ActionEventListener listener) : base(context, listener) { }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Get, QQConstants.URL_GET_QRCODE);
            req.AddGetValue("appid", QQConstants.APPID);
            req.AddGetValue("e", "0");
            req.AddGetValue("l", "M");
            req.AddGetValue("s", "5");
            req.AddGetValue("d", "72");
            req.AddGetValue("v", "4");
            req.AddGetValue("t", new Random().NextDouble());
            //req.AddRefer(QQConstants.URL_LOGIN_PAGE);
            //req.AddHeader(HttpConstants.SetCookie, "qrsig=dG0lVGD8IhpDl1cMsy4qgghLk24rOwSK9YVq2YlWAjBzJ69tIE-9sFkMttULkrww; PATH=/; DOMAIN=ptlogin2.qq.com;");
            req.ResultType = ResponseResultType.Stream;
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            NotifyActionEvent(ActionEventType.EvtOK, Image.FromStream(responseItem.ResponseStream));
        }
    }
}
