using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetOnlineBuddiesAction : QQAction
    {
        public GetOnlineBuddiesAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetQRCode);
            req.AddQueryValue("vfwebqq", Session.Vfwebqq);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", Session.SessionId);
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.Referrer;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJsonObj();
            if (json["retcode"].ToString() == "0")
            {
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
