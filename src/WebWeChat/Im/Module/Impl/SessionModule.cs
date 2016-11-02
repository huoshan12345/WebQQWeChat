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

        public SessionState State { get; set; } = SessionState.Offline;

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

        public string DeviceId
        {
            get { return BaseRequest.GetOrDefault(nameof(DeviceId)); }
            set
            {
                BaseRequest[nameof(DeviceId)] = value;
            }
        }

        /// <summary>
        /// 基础请求参数
        /// 之所以放到dic里面，是因为很多的请求都需要这几个参数，所以放在一起方便json序列化
        /// </summary>
        public IDictionary<string, string> BaseRequest { get; } = new Dictionary<string, string>();

        public SessionModule(IWeChatContext context) : base(context)
        {
            var seed = new Random().NextDouble();
            DeviceId = $"e{ seed.ToString("f15").Split('.')[1] }";
        }
    }
}
