using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Extensions;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetVfwebqqAction : WebQQAction
    {
        public GetVfwebqqAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetVfwebqq);
            req.AddQueryValue("ptwebqq", Session.Ptwebqq);
            req.AddQueryValue("clientid", Session.ClientId);
            req.AddQueryValue("psessionid", "");
            req.AddQueryValue("t", Timestamp);
            req.Referrer = ApiUrls.ReferrerS;
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                var ret = json["result"];
                Session.Vfwebqq = ret["vfwebqq"].ToString();
                return NotifyOkEventAsync();
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
