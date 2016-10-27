using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Utility.Extensions;

namespace WebWeChat.Im.Module.Impl
{
    public class SessionModule : AbstractModule
    {
        public string Uuid { get; set; }

        public string BaseUrl { get; set; }

        public string LoginUrl { get; set; }

        public string Skey
        {
            get { return BaseRequest.GetOrDefault(nameof(Skey)); }
            set { BaseRequest[nameof(Skey)] = value; }
        }

        public IDictionary<string, string> BaseRequest { get; } = new Dictionary<string, string>();
    }
}
