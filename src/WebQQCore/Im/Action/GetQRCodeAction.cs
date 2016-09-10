using System;
using System.Drawing;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetQRCodeAction : AbstractHttpAction
    {
        public GetQRCodeAction(IQQContext context, QQActionListener listener) : base(context, listener) { }


        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_QRCODE);
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

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EvtOK, Image.FromStream(response.ResponseStream));
        }
    }
}
