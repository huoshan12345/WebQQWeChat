using System.Text.RegularExpressions;
using HttpActionTools.Core;
using HttpActionTools.Event;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class CheckQRCodeAction : AbstractWebQQAction
    {
        public CheckQRCodeAction(IQQContext context, ActionEventListener listener) : base(context, listener) { }
        
        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Get, QQConstants.URL_CHECK_QRCODE);
            req.AddGetValue("webqq_type", "10");
            req.AddGetValue("remember_uin", "1");
            req.AddGetValue("login2qq", "1");
            req.AddGetValue("aid", "501004106");
            req.AddGetValue("u1", "http://w.qq.com/proxy.html?login2qq=1&webqq_type=10");
            req.AddGetValue("ptredirect", "0");
            req.AddGetValue("ptlang", "2052");
            req.AddGetValue("daid", "164");
            req.AddGetValue("from_ui", "1");
            req.AddGetValue("pttype", "1");
            req.AddGetValue("dumy", "");
            req.AddGetValue("fp", "loginerroralert");
            req.AddGetValue("action", "0-0-205298");
            req.AddGetValue("mibao_css", "m_webqq");
            req.AddGetValue("t", "1");
            req.AddGetValue("g", "1");
            req.AddGetValue("js_type", "0");
            req.AddGetValue("js_ver", QQConstants.JSVER);
            req.AddGetValue("login_sig", "");
            req.AddGetValue("pt_randsalt", "0");
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            var rex = new Regex(QQConstants.REGXP_LOGIN);
            var str = responseItem.ResponseString;
            var m = rex.Match(str);
            if (m.Success)
            {
                var ret = m.Groups[1].Value;
                switch (ret)
                {
                    //case "0": NotifyActionEvent(QQActionEventType.EVT_OK, m.Groups[3].Value); break;
                    //case "66": throw new QQException(QQErrorCode.QRCODE_OK, m.Groups[5].Value);
                    //case "67": throw new QQException(QQErrorCode.QRCODE_AUTH, m.Groups[5].Value);
                    case "0": NotifyActionEvent(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.OK, m.Groups[3].Value)); break;
                    case "66": NotifyActionEvent(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Valid, m.Groups[5].Value)); break;
                    case "67": NotifyActionEvent(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Auth, m.Groups[5].Value)); break;
                    case "65":NotifyActionEvent(ActionEventType.EvtOK, new CheckQRCodeArgs(QRCodeStatus.Invalid, m.Groups[5].Value)); break;

                    default: throw new QQException(QQErrorCode.InvalidUser, m.Groups[5].Value);
                }
            }
            else
            {
                throw new QQException(QQErrorCode.UnexpectedResponse, str);
            }
        }

    }

}
