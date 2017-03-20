using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction.Service;

namespace HttpAction.Action
{
    public abstract class AbstractDownloadAction : AbstractHttpAction
    {
        protected readonly string _filePath;

        protected abstract string GetUrl();

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateGetRequest(GetUrl());
            req.ResultType = HttpResultType.Byte;
            ModifyRequest(req);
            return req;
        }

        protected virtual void ModifyRequest(HttpRequestItem req) { }

        protected AbstractDownloadAction(IHttpService httpHttpService, string filePath) : base(httpHttpService)
        {
            _filePath = filePath;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            File.WriteAllBytes(_filePath, response.ResponseBytes);
            return NotifyOkEventAsync(_filePath);
        }
    }
}
