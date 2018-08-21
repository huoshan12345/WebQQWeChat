using System.Threading.Tasks;
using HttpAction.Core;
using FclEx.Http.Event;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class CheckSigAction : WebQQAction
    {
        public CheckSigAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override string Url => Session.CheckSigUrl;

        protected override HttpReqType ReqType { get; } = HttpReqType.Get;

        protected override ValueTask<ActionEvent> HandleResponse(HttpRes response)
        {
            var ptwebqq = HttpService.GetCookie("ptwebqq", Session.CheckSigUrl);
            ptwebqq.Expired = true;
            //Session.Ptwebqq = ptwebqq;
            return NotifyOkEventAsync();
        }
    }
}
