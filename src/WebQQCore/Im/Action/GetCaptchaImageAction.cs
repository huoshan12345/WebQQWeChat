using System;
using System.Drawing;
using System.IO;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取验证码图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetCaptchaImageAction : AbstractHttpAction
    {
        private readonly long _uin;

        public GetCaptchaImageAction(IQQContext context, QQActionListener listener, long uin)
            : base(context, listener)
        {
            _uin = uin;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EvtOK, Image.FromStream(response.ResponseStream));
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_CAPTCHA);
            req.AddGetValue("aid", QQConstants.APPID);
            req.AddGetValue("r", new Random().NextDouble().ToString("f16"));
            req.AddGetValue("uin", _uin);

            // 20150724增加
            req.AddGetValue("cap_cd", Context.Session.CapCd);
            req.ResultType = ResponseResultType.Stream;
            return req;
        }
    }
}
