using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpActionTools.Extensions;

namespace HttpActionTools.Core
{
    public class HttpRequestItem
    {
        public string RawUrl { get; set; }
        public Encoding EncodingType { get; set; }
        public Dictionary<string, string> HeaderMap { get; }
        public Dictionary<string, string> QueryMap { get; set; }
        public HttpMethodType Method { get; set; }
        public ResponseResultType ResultType { set; get; }

        public string ContentType
        {
            get { return HeaderMap[HttpConstants.ContentType]; }
            set { HeaderMap[HttpConstants.ContentType] = value; }
        }

        public string Referrer
        {
            get { return HeaderMap.GetValueOrDefault(HttpConstants.Referrer); }
            set { HeaderMap[HttpConstants.Referrer] = value; }
        }

        public string Origin
        {
            get { return HeaderMap.GetValueOrDefault(HttpConstants.Origin); }
            set { HeaderMap[HttpConstants.Origin] = value; }
        }

        public HttpRequestItem(string rawUrl, HttpMethodType method)
        {
            RawUrl = rawUrl;
            Method = method;
            HeaderMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [HttpConstants.ContentType] = Method == HttpMethodType.Post
                    ? HttpConstants.DefaultPostContentType
                    : HttpConstants.DefaultGetContentType,
            };
            QueryMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            EncodingType = Encoding.UTF8;
        }

        public string GetQueryString()
        {
            return QueryMap.IsNullOrEmpty() ? string.Empty : 
                string.Join("&", QueryMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

        public string GetUrlWithQuery()
        {
            return QueryMap.IsNullOrEmpty() ? RawUrl : 
                $"{RawUrl}?{GetQueryString()}";
        }

        public string GetUrl()
        {
            return Method == HttpMethodType.Get ? GetUrlWithQuery() : RawUrl;
        }
    }
}
