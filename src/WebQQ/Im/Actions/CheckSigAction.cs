using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class CheckSigAction : WebQQAction
    {
        public CheckSigAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override string Url => Session.CheckSigUrl;

        protected override EnumRequestType RequestType { get; } = EnumRequestType.Get;

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var ptwebqq = HttpService.GetCookie("ptwebqq", Session.CheckSigUrl);
            ptwebqq.Expired = true;
            //Session.Ptwebqq = ptwebqq;
            return NotifyOkEventAsync();
        }
    }
}
