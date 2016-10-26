namespace HttpActionFrame.Core
{
    public abstract class HttpConstants
    {
        public const string Location = "Location";
        public const string UserAgent = "User-Agent";
        public const string Referrer = "Referer";
        public const string Post = "POST";
        public const string Get = "GET";
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string SetCookie = "Set-Cookie";
        public const string Origin = "Origin";
        public const string Cookie = "Cookie";
        public const string DefaultGetContentType = "text/html";
        public const string DefaultPostContentType = "application/x-www-form-urlencoded";
        public const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 UBrowser/5.7.15319.202 Safari/537.36";
    }

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

    public enum ProxyType
    {
        Http,
        Sock4,
        Sock5
    }
}
