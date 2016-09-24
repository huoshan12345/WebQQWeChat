using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HttpActionFrame.Extensions;
using System.Linq;

namespace HttpActionFrame.Core
{
    public class HttpResponseItem
    {
        public HttpRequestItem RequestItem { set; get; }
        public string ResponseString { set; get; }
        public byte[] ResponseBytes { set; get; }
        public Stream ResponseStream { set; get; }
        public Dictionary<string, List<string>> Headers { set; get; } = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        public string Location => Headers.GetValueOrDefault(HttpConstants.Location).FirstOrDefault();
        public Exception Exception { set; get; }
        public bool Success => Exception == null;
        public HttpStatusCode StatusCode { set; get; }
    }
}
