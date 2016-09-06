using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util
{
    public enum HttpMethodType
    {
        Get,
        Post,
        Put,
        Delete,
        Head,
        Options,
        Trace
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResponseResultType
    {
        String,
        Byte,
        Stream,
    }

    public abstract class HttpConstants
    {
        public const string UserAgent = "User-Agent";
        public const string Referrer = "Referer";
        public const string Post = "POST";
        public const string Get = "GET";
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string SetCookie = "Set-Cookie";
        public const string Origin = "Origin";
        public const string Cookie = "Cookie";
        public const string DefaultGetContentType = "application/json; charset=utf-8";
        public const string DefaultPostContentType = "application/x-www-form-urlencoded";
    }
}
