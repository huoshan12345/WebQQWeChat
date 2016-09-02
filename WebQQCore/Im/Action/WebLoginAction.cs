using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>登录</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class WebLoginAction : AbstractHttpAction
    {
        private readonly string _username;
        private readonly string _password;
        private readonly long _uin;
        private readonly string _verifyCode;

        public WebLoginAction(IQQContext context, QQActionListener listener, 
            string username, string password, long uin, string verifyCode)
            : base(context, listener)
        {
            _username = username;
            _password = password;
            _uin = uin;
            _verifyCode = verifyCode;
        }

        public override QQHttpRequest BuildRequest()
        {
            /*
                u:1070772010
                p:D2E8ECC0E10185EFAEECFD3532BC34F7
                verifycode:dsads
                webqq_type:10
                remember_uin:1
                login2qq:1
                aid:1003903
                u1:http://web2.qq.com/loginproxy.html?login2qq=1&webqq_type=10
                h:1
                ptredirect:0
                ptlang:2052
                daid:164
                from_ui:1
                pttype:1
                dumy:
                fp:loginerroralert
                action:4-28-1632882
                mibao_css:m_webqq
                t:1
                g:1
                js_type:0
                js_ver:10038
                login_sig:a4YzJkO9z19WM0-M6fZ9rRGyo7QhwGz7GjiQW4MiSdxldWj9uNf8D9D1DAZNlMqF
             */

            var qqpwd = QQEncryptor.EncryptQQ(_uin, _password, _verifyCode);
            // Context.Logger.Info($"qq信息加密后是{qqpwd}");

            //尝试登录，准备传递的参数值
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_UI_LOGIN);
            req.AddGetValue("u", _username);
            req.AddGetValue("p", qqpwd);
            req.AddGetValue("verifycode", _verifyCode);
            req.AddGetValue("webqq_type", "10");
            req.AddGetValue("remember_uin", "1");
            req.AddGetValue("login2qq", "0");
            req.AddGetValue("aid", QQConstants.APPID);
            req.AddGetValue("u1", "http://web2.qq.com/loginproxy.html?login2qq=0&webqq_type=10");
            req.AddGetValue("h", "1");
            req.AddGetValue("ptredirect", "0");
            req.AddGetValue("ptlang", "2052");
            req.AddGetValue("daid", "164");
            req.AddGetValue("from_ui", "1");
            req.AddGetValue("pttype", "1");
            req.AddGetValue("dumy", "");
            req.AddGetValue("fp", "loginerroralert");
            req.AddGetValue("action", "2-9-8318");
            req.AddGetValue("mibao_css", "m_webqq");
            req.AddGetValue("t", "1");
            req.AddGetValue("g", "1");
            req.AddGetValue("js_type", "0");
            req.AddGetValue("js_ver", QQConstants.JSVER);
            req.AddGetValue("login_sig", Context.Session.LoginSig);


            //2015-03-02 登录协议增加的参数
            req.AddGetValue("pt_uistyle", "5");
            req.AddGetValue("pt_randsalt", "0");
            req.AddGetValue("pt_vcode_v1", "0");

            var httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            var ptvfsession = httpService.GetCookie("ptvfsession", QQConstants.URL_CHECK_VERIFY)
                ?? httpService.GetCookie("verifysession", QQConstants.URL_CHECK_VERIFY);// 验证session在获取验证码阶段得到的。

            if (ptvfsession != null) req.AddGetValue("pt_verifysession_v1", ptvfsession.Value);
            else Context.Logger.Info("pt_verifysession_v1为空");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var rex = new Regex(QQConstants.REGXP_LOGIN);
            var mc = rex.Match(response.GetResponseString());

            if (mc.Success)
            {
                var ret = int.Parse(mc.Groups[1].Value);
                switch (ret)
                {
                    case 0:
                    NotifyActionEvent(QQActionEventType.EvtOK, mc.Groups[3].Value);
                    break;

                    case 3:
                    throw new QQException(QQErrorCode.WrongPassword, mc.Groups[5].Value);

                    case 4:
                    throw new QQException(QQErrorCode.WrongCaptcha, mc.Groups[5].Value);

                    case 7:
                    throw new QQException(QQErrorCode.IOError, mc.Groups[5].Value);

                    default:
                    throw new QQException(QQErrorCode.InvalidUser, mc.Groups[5].Value);
                }
            }
            else
            {
                throw new QQException(QQErrorCode.UnexpectedResponse);
            }
        }

    }

}
