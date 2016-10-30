using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Utility.Extensions;

namespace HttpActionFrame.Core
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

        public string RawUrl { get; }
        public Encoding EncodingType { get; set; }
        public HttpMethodType Method { get; set; }
        public ResponseResultType ResultType { get; set; }
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
            RawUrl = rawUrl;
            Method = method;
            HeaderMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //[HttpConstants.ContentType] = Method == HttpMethodType.Post
                //    ? HttpConstants.DefaultPostContentType : HttpConstants.DefaultGetContentType,
                // [HttpConstants.Host] = new Uri(rawUrl).Host,
            };
            if (Method != HttpMethodType.Get)
            {
                HeaderMap[HttpConstants.ContentType] = HttpConstants.DefaultPostContentType;
            }
            _rawData = new StringBuilder();
            EncodingType = Encoding.UTF8;
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
            _rawData.Append($"{key.UrlEncode()}={value.SafeToString().UrlEncode()}");
        }

        public void AddQueryString(string str)
        {
            if (_rawData.Length != 0) _rawData.Append("&");
            _rawData.Append(str);
        }

        //[Obsolete]
        //public void AddGetValue(string key, object value)
        //{
        //    AddQueryValue(key, value);
        //}

        //[Obsolete]
        //public void AddPostValue(string key, object value)
        //{
        //    AddQueryValue(key, value);
        //}

        public void AddHeader(string key, string value)
        {
            HeaderMap[key] = value;
        }
    }
}
