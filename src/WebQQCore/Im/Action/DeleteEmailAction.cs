using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// 删除Email
    /// </summary>
    public class DeleteEmailAction : AbstractHttpAction
    {
        private readonly List<QQEmail> _markList;

        public DeleteEmailAction(List<QQEmail> markList,
                IQQContext context, QQActionListener listener)
            : base(context, listener)
        {
            this._markList = markList;
        }

        public override QQHttpRequest BuildRequest()
        {
            // mailaction=mail_del&mailid=C1TFACD70BB&t=mail_mgr2&resp_charset=UTF8&ef=js&sid=eEVNdM8QDlC8YWEz&folderkey=1
            var req = CreateHttpRequest("POST", QQConstants.URL_MARK_EMAIL);
            req.AddPostValue("mailaction", "mail_del");
            req.AddPostValue("t", "mail_mgr2");
            req.AddPostValue("resp_charset", "UTF8");
            req.AddPostValue("ef", "js");
            req.AddPostValue("folderkey", "1");
            req.AddPostValue("sid", Context.Session.EmailAuthKey);
            foreach (var mail in _markList)
            {
                req.AddPostValue("mailid", mail.Id);
            }
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // ({msg : "new successful",rbkey : "1391255617",status : "false"})
            var str = response.GetResponseString();
            // LOG.info("delete email: " + ct);
            if (str.Contains("success"))
            {
                NotifyActionEvent(QQActionEventType.EvtOK, str);
            }
            else
            {
                // NotifyActionEvent(QQActionEventType.EVT_ERROR, str);
                throw new QQException(QQErrorCode.UnexpectedResponse, str);
            }
        }

    }

}
