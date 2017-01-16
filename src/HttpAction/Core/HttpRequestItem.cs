using System;
using System.Collections.Generic;
using System.Text;
using FclEx.Extensions;

namespace HttpAction.Core
{
    public class HttpRequestItem
    {
        public string RawData
        {
            get { return _rawData.ToString(); }
            set
            {
                if (_rawData.Length != 0) _rawData.Clear();
                _rawData.Append(value);
            }
        }

        public Uri Uri { get; }
        public string RawUrl => Uri.AbsoluteUri;
        public Encoding EncodingType { get; set; }
        public HttpMethodType Method { get; set; }
        public HttpResultType ResultType { get; set; }
        public Dictionary<string, string> HeaderMap { get; }
        private readonly StringBuilder _rawData;

        public string ContentType
        {
            get { return HeaderMap.GetOrDefault(HttpConstants.ContentType); }
            set { HeaderMap[HttpConstants.ContentType] = value; }
        }

        public string Referrer
        {
            get { return HeaderMap.GetOrDefault(HttpConstants.Referrer); }
            set { HeaderMap[HttpConstants.Referrer] = value; }
        }

        public string Origin
        {
            get { return HeaderMap.GetOrDefault(HttpConstants.Origin); }
            set { HeaderMap[HttpConstants.Origin] = value; }
        }

        public HttpRequestItem(HttpMethodType method, string rawUrl)
        {
            Uri = new Uri(rawUrl);
            // RawUrl = rawUrl;
            Method = method;
            HeaderMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [HttpConstants.ContentType] = Method == HttpMethodType.Post
                    ? HttpConstants.DefaultPostContentType : HttpConstants.DefaultGetContentType,
                [HttpConstants.Host] = Uri.Host,
            };
            if (Method != HttpMethodType.Get)
            {
                HeaderMap[HttpConstants.ContentType] = HttpConstants.DefaultPostContentType;
            }
            _rawData = new StringBuilder();
            EncodingType = Encoding.UTF8;
        }

        public static HttpRequestItem CreateJsonRequest(string rawUrl)
        {
            return new HttpRequestItem(HttpMethodType.Post, rawUrl)
            {
                ContentType = HttpConstants.JsonContentType
            };
        }

        public static HttpRequestItem CreateFormRequest(string url)
        {
            return new HttpRequestItem(HttpMethodType.Post, url);
        }

        public static HttpRequestItem CreateGetRequest(string url)
        {
            return new HttpRequestItem(HttpMethodType.Get, url);
        }

        public HttpRequestItem(HttpMethodType method, string rawUrl, IDictionary<string, string> queryValues)
            : this(method, rawUrl)
        {
            _rawData.Append(queryValues.ToQueryString());
        }

        public string GetUrlWithQuery()
        {
            return _rawData.Length == 0 ? RawUrl :
                $"{RawUrl}?{RawData}";
        }

        public string GetUrl()
        {
            return Method == HttpMethodType.Get ? GetUrlWithQuery() : RawUrl;
        }

        public void AddQueryValue(string key, object value)
        {
            if (_rawData.Length != 0) _rawData.Append("&");
            var valueStr = (value as string) ?? value.SafeToString();
            _rawData.Append($"{key.UrlEncode()}={valueStr.UrlEncode()}");
        }

        public void AddQueryString(string str)
        {
            if (_rawData.Length != 0) _rawData.Append("&");
            _rawData.Append(str);
        }

        public void AddHeader(string key, string value)
        {
            HeaderMap[key] = value;
        }
    }
}
