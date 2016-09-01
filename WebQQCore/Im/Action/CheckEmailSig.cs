using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>通过pt4获取到的URL进行封装</para>
    /// <para>检测邮箱是否合法登录了</para>
    /// <para>承∮诺</para>
    /// </summary>
    public class CheckEmailSig : AbstractHttpAction
    {
        private string _url;
        public CheckEmailSig(string url, IQQContext context, QQActionEventHandler listener)
            : base(context, listener)
        {
            this._url = url;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            _url += "&regmaster=undefined&aid=1";
            _url += "&s_url=http%3A%2F%2Fmail.qq.com%2Fcgi-bin%2Flogin%3Ffun%3Dpassport%26from%3Dwebqq";

            var req = CreateHttpRequest(HttpConstants.Get, _url);
            return req;
        }

        

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EvtOK, "");
        }
    }
}
