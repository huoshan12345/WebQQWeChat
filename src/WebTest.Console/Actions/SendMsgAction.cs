using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Action;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;
using WebTest.Models;
using WebTest.Models.QQModels;

namespace WebTest.Actions
{
    public class SendMsgAction : AbstractHttpAction
    {
        private readonly string _token;
        private readonly QQMessageModel _msg;

        public SendMsgAction(string token, QQMessageModel msg, IHttpService httpService, ActionEventListener listener = null) : base(httpService, listener)
        {
            _token = token;
            _msg = msg;
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateJsonRequest(ApiUrls.SendMsg);
            req.SetBearerToken(_token);
            req.RawData = _msg.ToJson();
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var result = response.ResponseString.ToJToken().ToObject<DataResult>();
            if (result.Successful) return NotifyOkEventAsync();
            else return NotifyErrorEventAsync(result.Error);
        }
    }
}
