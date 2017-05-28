using System;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Action;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;
using Newtonsoft.Json.Linq;

namespace WebTest.Actions
{
    public class GetTokenAction : AbstractHttpAction
    {
        public GetTokenAction(IHttpService httpService, ActionEventListener listener = null) : base(httpService, listener)
        {
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.GetTokenUrl);
            req.AddQueryValue("username", "admin@admin.com");
            req.AddQueryValue("password", "Admin@12345");
            req.AddQueryValue("grant_type", "password");
            req.AddQueryValue("client_id", "api-client");
            req.AddQueryValue("client_secret", "secret");
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var token = response.ResponseString.ToJToken()["access_token"].ToString();
            return NotifyOkEventAsync(token);
        }
    }
}
