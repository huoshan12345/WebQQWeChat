using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>临时消息信道，用于发送群U2U会话消息</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetSessionMsgSigAction : AbstractHttpAction
    {

        private readonly QQStranger _user;

        public GetSessionMsgSigAction(IQQContext context, QQActionListener listener,
                QQStranger user)
            : base(context, listener)
        {
            this._user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get,
                    QQConstants.URL_GET_SESSION_MSG_SIG);
            if (_user is QQGroupMember)
            {
                var mb = _user as QQGroupMember;
                mb.ServiceType = 0;
                req.AddGetValue("id", mb.Group.Gin);
                req.AddGetValue("service_type", "0"); // 0为群，1为讨论组
            }
            else if (_user is QQDiscuzMember)
            {
                var mb = (QQDiscuzMember)_user;
                mb.ServiceType = 1;
                req.AddGetValue("id", mb.Discuz.Did);
                req.AddGetValue("service_type", "1"); // 0为群，1为讨论组
            }
            else
            {
                // LOG.info("GetSessionMsgSigAction unknow type :" + user);
            }
            req.AddGetValue("to_uin", _user.Uin);
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // {"retcode":0,"result":{"type":0,"value":"sig","flags":{"text":1,"pic":1,"file":1,"audio":1,"video":1}}}
            var json = JObject.Parse(response.GetResponseString());
            var retcode = json["retcode"].ToObject<int>();

            if (retcode == 0)
            {
                var result = json["result"].ToObject<JObject>();
                if (result["value"] != null)
                {
                    _user.GroupSig = result["value"].ToString();
                    NotifyActionEvent(QQActionEventType.EvtOK, _user);
                    return;
                }
            }

            NotifyActionEvent(QQActionEventType.EvtError, new QQException(
                    QQErrorCode.UnexpectedResponse, JsonConvert.SerializeObject(json)));
        }
    }

}
