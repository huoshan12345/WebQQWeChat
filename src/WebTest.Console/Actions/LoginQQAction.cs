using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HttpAction.Action;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;

namespace WebTest.Actions
{
    public class LoginQQAction : AbstractHttpAction
    {
        private readonly string _token;

        public LoginQQAction(string token, IHttpService httpService, ActionEventListener listener = null) : base(httpService, listener)
        {
            _token = token;
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.LoginQQUrl);
            req.AddHeader("Authorization", "Bearer " + _token);
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            return NotifyOkEventAsync(response.ResponseString);
        }
    }
}
