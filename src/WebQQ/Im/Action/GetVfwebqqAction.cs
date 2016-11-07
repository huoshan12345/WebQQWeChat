using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetVfwebqqAction:QQAction
    {
        public GetVfwebqqAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetVfwebqq);
            req.AddQueryValue("ptwebqq", Session.Ptwebqq);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", "");
            req.AddQueryValue("t", Timestamp);
            req.Referrer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJsonObj();
            if (json["retcode"].ToString() == "0")
            {
                var ret = json["result"].ToJsonObj();
                Session.Vfwebqq = ret["vfwebqq"].ToString();
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
