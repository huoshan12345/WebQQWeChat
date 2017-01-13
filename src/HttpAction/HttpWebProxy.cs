using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HttpAction
{
    public class HttpWebProxy : IWebProxy
    {
        private readonly Uri _uri;

        public HttpWebProxy(Uri uri)
        {
            _uri = uri;
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination) => _uri;

        public bool IsBypassed(Uri host) => false;
    }
}
