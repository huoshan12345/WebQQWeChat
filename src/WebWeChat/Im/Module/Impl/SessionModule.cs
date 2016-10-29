using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Utility.Extensions;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Module.Impl
{
    public class SessionModule : WeChatModule
    {
        public SessionModule()
        {
            var seed = new Random().NextDouble();
            DeviceId = $"e{ seed.ToString("f15").Split('.')[1] }";
        }

        public SessionState State { get; set; } = SessionState.Offline;

        public string Uuid { get; set; }

        public string BaseUrl { get; set; }

        public string LoginUrl { get; set; }

        public string SyncUrl { get; set; }

        public string PassTicket { get; set; }

        public JToken SyncKey { get; set; }

        public string SyncKeyStr { get; set; }

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
            set { BaseRequest[nameof(DeviceId)] = value; }
        }

        /// <summary>
        /// 基础请求参数
        /// 之所以放到dic里面，是因为很多的请求都需要这几个参数，所以放在一起方便json序列化
        /// </summary>
        public IDictionary<string, string> BaseRequest { get; } = new Dictionary<string, string>();
    }
}
