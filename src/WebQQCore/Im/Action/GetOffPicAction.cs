using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取聊天图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetOffPicAction : AbstractHttpAction
    {
        private readonly OffPicItem _offpic;
        private readonly QQMsg _msg;
        private readonly Stream _picOut;

        public GetOffPicAction(IQQContext context, QQActionListener listener,
            OffPicItem offpic, QQMsg msg, Stream picOut)
            : base(context, listener)
        {

            _offpic = offpic;
            _msg = msg;
            _picOut = picOut;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EvtOK, _offpic);
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_OFFPIC);
            var session = Context.Session;
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("f_uin", _msg.From.Uin);
            req.AddGetValue("file_path", _offpic.FilePath);
            req.AddGetValue("psessionid", session.SessionId);
            req.OutputStream = _picOut;
            return req;
        }
        
    }
}
