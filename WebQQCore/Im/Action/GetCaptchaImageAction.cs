using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取验证码图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetCaptchaImageAction : AbstractHttpAction
    {
        private readonly long _uin;

        public GetCaptchaImageAction(QQContext context, QQActionEventHandler listener, long uin)
            : base(context, listener)
        {
            this._uin = uin;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            try
            {
                MemoryStream ms = new MemoryStream(response.ResponseData);
                NotifyActionEvent(QQActionEventType.EVT_OK, Image.FromStream(ms));
            }
            catch (IOException e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNKNOWN_ERROR, e));
            }
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_CAPTCHA);
            req.AddGetValue("aid", QQConstants.APPID);
            req.AddGetValue("r", new Random().NextDouble().ToString("f16"));
            req.AddGetValue("uin", _uin + "");

            // 20150724增加
            req.AddGetValue("cap_cd", Context.Session.CapCd);
            return req;
        }
    }
}
