using System;
using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>邮件轮询</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月25日</para>
    /// </summary>
    public class PollEmailAction : AbstractHttpAction
    {


        private readonly string _sid;
        private readonly long _t;

        public PollEmailAction(string sid, long t, IQQContext context, QQActionListener listener)
            : base(context, listener)
        {
            _sid = sid;
            _t = t;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_EMAIL_POLL);
            req.AddGetValue("r", new Random().NextDouble());
            req.AddGetValue("u", Context.Account.Username);
            req.AddGetValue("s", "7");
            req.AddGetValue("k", _sid);
            req.AddGetValue("t", _t);
            req.AddGetValue("i", "30");
            req.AddGetValue("r", new Random().NextDouble());
            req.ReadTimeout = 70 * 1000;
            req.ConnectTimeout = 10 * 1000;
            req.AddHeader("Referer", "http://wp.mail.qq.com/ajax_proxy.html?mail.qq.com&v=110702");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var content = response.GetResponseString();
            // LOG.info("Poll email content: " + content);
            if (content.StartsWith("({e:-101"))
            {
                // 空，没有新邮件
                NotifyActionEvent(QQActionEventType.EvtOK, null);
            }
            else if (content.StartsWith("({e:-100"))
            {
                // 凭证已经失效，需要重新登录或者获取wpkey	
                NotifyActionEvent(QQActionEventType.EvtError,
                        new QQException(QQErrorCode.INVALID_LOGIN_AUTH, content));
            }
            else
            {
                content = content.Substring(1, content.Length - 1);
                var arr = JArray.Parse(content);
                // 封装返回的邮件列表
                var list = new List<QQEmail>();
                for (var i = 0; i < arr.Count; i++)
                {
                    var json = arr[i].ToObject<JObject>();
                    var ct = json["c"].ToObject<JObject>();
                    var mail = new QQEmail();
                    mail.Flag = json["t"].ToObject<long>();
                    mail.Id = ct["mailid"].ToString();
                    mail.Sender = ct["sender"].ToString();
                    mail.SenderNick = ct["senderNick"].ToString();
                    mail.SenderEmail = ct["senderEmail"].ToString();
                    mail.Subject = ct["subject"].ToString();
                    mail.Summary = ct["summary"].ToString();
                    mail.Unread = true;
                    list.Add(mail);
                }
                NotifyActionEvent(QQActionEventType.EvtOK, new QQNotifyEvent(QQNotifyEventType.EmailNotify, list));
            }
        }

    }

}
