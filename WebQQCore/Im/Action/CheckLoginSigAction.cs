using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>CheckLoginSigAction</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class CheckLoginSigAction : AbstractHttpAction
    {
        private readonly string _checkSigUrl;
 
        public CheckLoginSigAction(IQQContext context, QQActionEventHandler listener, string checkSigUrl)
            : base(context, listener)
        {
            _checkSigUrl = checkSigUrl;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // LOG.info("checkSig result:" + response.GetResponseString());
            // LOG.info("Location:" + response.getHeader("Location"));
            NotifyActionEvent(QQActionEventType.EVT_OK, null);
        }

        public override QQHttpRequest OnBuildRequest()
        {
            return CreateHttpRequest(HttpConstants.Get, _checkSigUrl);
        }

    }

}
