using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HttpAction;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Util;

namespace WebQQ.Im.Actions
{
    public class CheckQRCodeAction : WebQQAction
    {
        private const string JsVer = "10194";
        private readonly Regex _reg = new Regex(@"ptuiCB\('(\d+)','(\d+)','(.*?)','(\d+)','(.*?)', '(.*?)'\)");

        public CheckQRCodeAction(IQQContext context, ActionEventListener listener = null) : base(context, listener) { }

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Get;

        protected override void ModifyRequest(HttpRequestItem req)
        {
            var qrsig = HttpService.GetCookie("qrsig", ApiUrls.CheckQRCode).Value;
            req.AddData("ptqrtoken", QQEncryptor.GetPtqrtoken(qrsig));
            req.AddData("webqq_type", "10");
            req.AddData("remember_uin", "1");
            req.AddData("login2qq", "1");
            req.AddData("aid", "501004106");
            req.AddData("u1", "http://w.qq.com/proxy.html?login2qq=1&webqq_type=10");
            req.AddData("ptredirect", "0");
            req.AddData("ptlang", "2052");
            req.AddData("daid", "164");
            req.AddData("from_ui", "1");
            req.AddData("pttype", "1");
            req.AddData("dumy", "");
            req.AddData("fp", "loginerroralert");
            req.AddData("action", "0-0-10194");
            req.AddData("mibao_css", "m_webqq");
            req.AddData("t", "1");
            req.AddData("g", "1");
            req.AddData("js_type", "0");
            req.AddData("js_ver", JsVer);
            req.AddData("login_sig", "");
            req.AddData("pt_randsalt", "0");
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var str = response.ResponseString;
            var m = _reg.Match(str);
            if (m.Success)
            {
                var ret = m.Groups[1].Value;
                switch (ret)
                {
                    case "0": return NotifyOkEventAsync(new CheckQRCodeArgs(QRCodeStatus.Ok, m.Groups[3].Value)); 
                    case "66": return NotifyOkEventAsync(new CheckQRCodeArgs(QRCodeStatus.Valid, m.Groups[5].Value));
                    case "67": return NotifyOkEventAsync(new CheckQRCodeArgs(QRCodeStatus.Auth, m.Groups[5].Value)); 
                    case "65": return NotifyOkEventAsync(new CheckQRCodeArgs(QRCodeStatus.Invalid, m.Groups[5].Value));
                }
            }
            return NotifyErrorEventAsync(QQErrorCode.ResponseError);
        }
    }

}
