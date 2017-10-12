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

        protected override HttpRequestItem BuildRequest()
        {
            return HttpRequestItem.CreateGetRequest(Session.CheckSigUrl);
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var ptwebqq = HttpService.GetCookie("ptwebqq", Session.CheckSigUrl).Value;
            Session.Ptwebqq = ptwebqq;
            return NotifyOkEventAsync();
        }
    }
}
