using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetQRCodeAction : AbstractHttpAction
    {

        /**
         * <p>Constructor for AbstractHttpAction.</p>
         *
         * @param context  a {@link IQQContext} object.
         * @param listener a {@link QQActionListener} object.
         */
        public GetQRCodeAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }


        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest("GET", QQConstants.URL_GET_QRCODE);
            req.AddGetValue("appid", QQConstants.APPID);
            req.AddGetValue("e", "0");
            req.AddGetValue("l", "M");
            req.AddGetValue("s", "5");
            req.AddGetValue("d", "72");
            req.AddGetValue("v", "4");
            req.AddGetValue("t", new Random().NextDouble());
            //req.AddRefer(QQConstants.URL_LOGIN_PAGE);
            //req.AddHeader(HttpConstants.SetCookie, "qrsig=dG0lVGD8IhpDl1cMsy4qgghLk24rOwSK9YVq2YlWAjBzJ69tIE-9sFkMttULkrww; PATH=/; DOMAIN=ptlogin2.qq.com;");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            try
            {
                var ms = new MemoryStream(response.ResponseData);
                NotifyActionEvent(QQActionEventType.EVT_OK, Image.FromStream(ms));
            }
            catch (IOException e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.IO_ERROR, e));
            }
            catch (Exception e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNKNOWN_ERROR, e));
            }
        }
    }
}
