using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Action;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;
using WebQQ.Im.Event;

namespace WebTest.Actions
{
    public class GetQQEventsAction : AbstractHttpAction
    {
        private readonly string _token;
        private readonly string _qqId;

        public GetQQEventsAction(string token, string qqId, IHttpService httpService, ActionEventListener listener = null) : base(httpService, listener)
        {
            _token = token;
            _qqId = qqId;
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(ApiUrls.GetAndClearEvents);
            req.AddHeader("Authorization", "Bearer " + _token);
            req.AddQueryValue("id", _qqId);
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var result = response.ResponseString.ToJToken().ToObject<IReadOnlyList<QQNotifyEvent>>();
            return NotifyOkEventAsync(result);
        }

        protected override async Task<ActionEvent> ExecuteInternalAsync(CancellationToken token)
        {
            Debug.WriteLine($"[Action={ActionName} Begin]");
            var result = await base.ExecuteInternalAsync(token).ConfigureAwait(false);
            Debug.WriteLine($"[Action={ActionName} End]");
            return result;
        }
    }
}
