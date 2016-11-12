using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class PollMsgAction : WebQQInfoAction
    {
        public PollMsgAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            var json = new JObject
            {
                {"clientid", Session.ClientId},
                {"psessionid", Session.SessionId},
                {"key", ""},
                {"ptwebqq", Session.Ptwebqq}
            };
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = ApiUrls.Referrer;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            return base.HandleResponse(response);
        }

        public override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            if (Session.State == SessionState.Online && (ex as QQException)?.ErrorCode == QQErrorCode.Timeout)
            {
                return Task.FromResult(ActionEvent.EmptyRepeatEvent);
            }
            else
            {
                return base.HandleExceptionAsync(ex);
            }
        }
    }
}
