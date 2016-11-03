using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Utility.Extensions;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Module.Impl
{
    public enum SessionState
    {
        Offline,
        Online,
    }

    public class SessionModule : WeChatModule
    {
        private readonly IDictionary<string, string> _baseRequest = new Dictionary<string, string>();
        private readonly Random _random = new Random();

        public SessionState State { get; set; } = SessionState.Offline;

        public long Seq { get; set; }

        public string Uuid { get; set; }

        public string BaseUrl { get; set; }

        public string LoginUrl { get; set; }

        public string SyncUrl { get; set; }

        public string PassTicket { get; set; }

        public JToken SyncKey { get; set; }

        public string SyncKeyStr => SyncKey?["List"].ToArray().Select(m => $"{m["Key"]}_{m["Val"]}").JoinWith("|");

        public JToken User { get; set; }

        public string Sid
        {
            get { return BaseRequest.GetOrDefault(nameof(Sid)); }
            set { BaseRequest[nameof(Sid)] = value; }
        }

        public string Uin
        {
            get { return BaseRequest.GetOrDefault(nameof(Uin)); }
            set { BaseRequest[nameof(Uin)] = value; }
        }

        public string Skey
        {
            get { return BaseRequest.GetOrDefault(nameof(Skey)); }
            set { BaseRequest[nameof(Skey)] = value; }
        }

        public string DeviceId => BaseRequest.GetOrDefault(nameof(DeviceId));

        /// <summary>
        /// 基础请求参数
        /// 之所以放到dic里面，是因为很多的请求都需要这几个参数，所以放在一起方便json序列化
        /// </summary>
        public IDictionary<string, string> BaseRequest
        {
            get
            {
                var seed = _random.NextDouble();
                var id = $"e{ seed.ToString("f15").Split('.')[1] }";
                _baseRequest[nameof(DeviceId)] = id;
                return _baseRequest;
            }
        }

        public SessionModule(IWeChatContext context) : base(context)
        {
            
        }
    }
}
