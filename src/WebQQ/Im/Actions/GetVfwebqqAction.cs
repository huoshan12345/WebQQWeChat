using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using FclEx.Http.Event;
using HttpAction;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetVfwebqqAction : WebQQAction
    {
        public GetVfwebqqAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Get;

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("ptwebqq", Session.Ptwebqq);
            req.AddData("clientid", Session.ClientId);
            req.AddData("psessionid", "");
            req.AddData("t", Timestamp);
            req.Referrer = ApiUrls.ReferrerS;
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
