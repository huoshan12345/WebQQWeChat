using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取聊天图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetOffPicAction : AbstractHttpAction
    {
        private OffPicItem offpic;
        private QQMsg msg;
        private Stream picOut;

        public GetOffPicAction(IQQContext context, QQActionEventHandler listener,
            OffPicItem offpic, QQMsg msg, Stream picOut)
            : base(context, listener)
        {

            this.offpic = offpic;
            this.msg = msg;
            this.picOut = picOut;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EVT_OK, offpic);
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_OFFPIC);
            QQSession session = Context.Session;
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("f_uin", msg.From.Uin);
            req.AddGetValue("file_path", offpic.FilePath);
            req.AddGetValue("psessionid", session.SessionId);
            req.OutputStream = picOut;
            return req;
        }
        
    }
}
