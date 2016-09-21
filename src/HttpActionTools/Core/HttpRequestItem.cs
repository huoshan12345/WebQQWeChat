using System;
using System.Collections.Generic;
using System.Text;
using HttpActionFrame.Extensions;
using System.Linq;

namespace HttpActionFrame.Core
{
    public class HttpRequestItem
    {
        public string RawUrl { get; set; }
        public Encoding EncodingType { get; set; }
        public Dictionary<string, string> _headerMap;
        public Dictionary<string, string> _queryMap;
        public HttpMethodType Method { get; set; }
        public ResponseResultType ResultType { get; set; }

        public string ContentType
        {
            get { return _headerMap[HttpConstants.ContentType]; }
            set { _headerMap[HttpConstants.ContentType] = value; }
        }

        public string Referrer
        {
            get { return _headerMap.GetValueOrDefault(HttpConstants.Referrer); }
            set { _headerMap[HttpConstants.Referrer] = value; }
        }

        public string Origin
        {
            get { return _headerMap.GetValueOrDefault(HttpConstants.Origin); }
            set { _headerMap[HttpConstants.Origin] = value; }
        }

        public HttpRequestItem(HttpMethodType method, string rawUrl)
        {
            RawUrl = rawUrl;
            Method = method;
            _headerMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [HttpConstants.ContentType] = Method == HttpMethodType.Post
                    ? HttpConstants.DefaultPostContentType
                    : HttpConstants.DefaultGetContentType,
            };
            _queryMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            EncodingType = Encoding.UTF8;
        }

        public string GetQueryString()
        {
            return _queryMap.IsNullOrEmpty() ? string.Empty : 
                string.Join("&", _queryMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

        public string GetUrlWithQuery()
        {
            return _queryMap.IsNullOrEmpty() ? RawUrl : 
                $"{RawUrl}?{GetQueryString()}";
        }

        public string GetUrl()
        {
            return Method == HttpMethodType.Get ? GetUrlWithQuery() : RawUrl;
        }

        public void AddQueryValue(string key, object value)
        {
            _queryMap[key] = value?.ToString() ?? string.Empty;
        }

        // [Obsolete]
        public void AddGetValue(string key, object value)
        {
            AddQueryValue(key, value);
        }
        
        // [Obsolete]
        public void AddPostValue(string key, object value)
        {
            AddQueryValue(key, value);
        }

        public void AddHeader(string key, string value)
        {
            _headerMap[key] = value;
        }
    }
}
