using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{

    public class MarkEmailAction : AbstractHttpAction
    {

        private bool status;
        private List<QQEmail> markList;

        public MarkEmailAction(bool status, List<QQEmail> markList,
                IQQContext context, QQActionEventHandler listener)
            : base(context, listener)
        {
            this.status = status;
            this.markList = markList;
        }

        public override QQHttpRequest BuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_MARK_EMAIL);
            req.AddPostValue("mailaction", "mail_flag");
            req.AddPostValue("flag", "new");
            req.AddPostValue("resp_charset", "UTF8");
            req.AddPostValue("ef", "js");
            req.AddPostValue("folderkey", "1");
            req.AddPostValue("sid", Context.Session.EmailAuthKey);
            req.AddPostValue("status", status + "");
            foreach (QQEmail mail in markList)
            {
                req.AddPostValue("mailid", mail.Id);
            }
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // ({msg : "new successful",rbkey : "1391255617",status : "false"})
            string ct = response.GetResponseString();
            if (ct.Contains("success"))
            {
                NotifyActionEvent(QQActionEventType.EVT_OK, ct);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, ct);
            }
        }
    }
}
