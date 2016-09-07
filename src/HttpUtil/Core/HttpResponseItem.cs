using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HttpActionTools.Extensions;

namespace HttpActionTools.Core
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
        public bool Success => Exception == null && StatusCode == HttpStatusCode.OK;
        public HttpStatusCode StatusCode { set; get; }
    }
}
