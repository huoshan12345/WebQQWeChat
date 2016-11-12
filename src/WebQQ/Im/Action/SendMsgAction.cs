using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;

namespace WebQQ.Im.Action
{
    public class SendMsgAction : WebQQAction
    {
        public SendMsgAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            HttpRequestItem req = null;
            // req.AddQueryValue("tuin", _friend.Uin);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("t", Timestamp);
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            throw new NotImplementedException();
        }
    }
}
