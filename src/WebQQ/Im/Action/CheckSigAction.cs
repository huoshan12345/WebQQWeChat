using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;

namespace WebQQ.Im.Action
{
    public class CheckSigAction:QQAction
    {
        public CheckSigAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            return HttpRequestItem.CreateGetRequest(Session.CheckSigUrl);
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            return NotifyActionEventAsync(ActionEventType.EvtOK);
        }
    }
}
