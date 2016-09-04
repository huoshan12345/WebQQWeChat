using System;
using System.Globalization;
using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>检查账号，如果账号安全则不需要验证码</para>
    /// <para>@author 承∮诺</para>
    /// </summary>
    public class CheckVerifyAction : AbstractHttpAction
    {
        private readonly string _qqAccount;

        public CheckVerifyAction(IQQContext context, QQActionListener listener, string qqAccount)
            : base(context, listener)
        {
            _qqAccount = qqAccount;
        }

        public override QQHttpRequest BuildRequest()
        {
            // "https://ssl.ptlogin2.qq.com/check?pt_tea=1&uin={0}&appid=" + APPID + "&js_ver=" + JSVER + "&js_type=0&login_sig={1}&u1=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html&r={2}"

            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_CHECK_VERIFY);
            req.AddGetValue("pt_tea", "1");
            req.AddGetValue("uin", _qqAccount);
            req.AddGetValue("appid", QQConstants.APPID);
            req.AddGetValue("js_ver", QQConstants.JSVER);
            req.AddGetValue("js_type", "0");
            req.AddGetValue("login_sig", Context.Session.LoginSig);
            req.AddGetValue("u1", "http://w.qq.com/proxy.html");
            req.AddGetValue("r", new Random().NextDouble().ToString("f16"));

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var rex = new Regex(QQConstants.REGXP_CHECK_VERIFY);
            var m = rex.Match(response.GetResponseString());
            if (m.Success)
            {
                var qqHex = m.Groups[3].Value;
                qqHex = Regex.Replace(qqHex, "\\\\x", "");
                var args = new CheckVerifyArgs
                {
                    Result = int.Parse(m.Groups[1].Value),
                    Code = m.Groups[2].Value,
                    Uin = long.Parse(qqHex, NumberStyles.HexNumber)
                };
                NotifyActionEvent(QQActionEventType.EvtOK, args);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, QQErrorCode.UnexpectedResponse);
            }
        }
    }

}
