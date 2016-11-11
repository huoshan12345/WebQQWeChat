using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;
using WebQQ.Im.Event;

namespace WebQQ.Im.Action
{
    public class CheckQRCodeAction : QQAction
    {
        private const string JsVer = "10153";
        private readonly Regex _reg = new Regex(@"ptuiCB\('(\d+)','(\d+)','(.*?)','(\d+)','(.*?)', '(.*?)'\)");

        public CheckQRCodeAction(IQQContext context, ActionEventListener listener = null) : base(context, listener) { }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Get, ApiUrls.CheckQRCode);
            req.AddQueryValue("webqq_type", "10");
            req.AddQueryValue("remember_uin", "1");
            req.AddQueryValue("login2qq", "1");
            req.AddQueryValue("aid", "501004106");
            req.AddQueryValue("u1", "http://w.qq.com/proxy.html?login2qq=1&webqq_type=10");
            req.AddQueryValue("ptredirect", "0");
            req.AddQueryValue("ptlang", "2052");
            req.AddQueryValue("daid", "164");
            req.AddQueryValue("from_ui", "1");
            req.AddQueryValue("pttype", "1");
            req.AddQueryValue("dumy", "");
            req.AddQueryValue("fp", "loginerroralert");
            req.AddQueryValue("action", "0-0-205298");
            req.AddQueryValue("mibao_css", "m_webqq");
            req.AddQueryValue("t", "1");
            req.AddQueryValue("g", "1");
            req.AddQueryValue("js_type", "0");
            req.AddQueryValue("js_ver", JsVer);
            req.AddQueryValue("login_sig", "");
            req.AddQueryValue("pt_randsalt", "0");
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var str = response.ResponseString;
            var m = _reg.Match(str);
            if (m.Success)
            {
                var ret = m.Groups[1].Value;
                switch (ret)
                {
                    case "0": return NotifyActionEventAsync(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.OK, m.Groups[3].Value)); 
                    case "66": return NotifyActionEventAsync(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Valid, m.Groups[5].Value));
                    case "67": return NotifyActionEventAsync(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Auth, m.Groups[5].Value)); 
                    case "65": return NotifyActionEventAsync(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Invalid, m.Groups[5].Value));
                }
            }
            return NotifyErrorEventAsync(QQErrorCode.ResponseError);
        }
    }

}
