using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Im.Http
{
    /**
     *
     * HTTP请求
     *
     * @author solosky
     */
    public class QQHttpRequest
    {
        private Stream _inputStream;

        public QQHttpRequest(string rawUrl, string method)
        {
            RawUrl = rawUrl;
            Method = method.ToUpper();
            HeaderMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PostMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            GetMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            FileMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            HeaderMap[HttpConstants.ContentType] = Method == HttpConstants.Post ? HttpConstants.DefaultPostContentType : HttpConstants.DefaultGetContentType;
            HeaderMap[HttpConstants.UserAgent] = QQConstants.USER_AGENT;
            HeaderMap[HttpConstants.Referrer] = QQConstants.REFFER;
        }

        public ResponseResultType ResultType { set; get; } = ResponseResultType.String;

        public string RawUrl { get; private set; }

        public string Url
        {
            get
            {
                if (GetMap.Count > 0)
                {
                    var query = string.Join("&", GetMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
                    return $"{RawUrl}?{query}";
                }
                else
                {
                    return RawUrl;
                }
            }
            set { RawUrl = value; }
        }

        public string Method { get; set; }

        public int Timeout { get; set; }

        public string PostBody { get; set; }

        public Stream OutputStream { get; set; }

        public string Charset { get; set; } = "utf-8";

        public int ConnectTimeout { get; set; } = 10 * 1000;

        public int ReadTimeout { get; set; } = 20 * 1000;

        public Dictionary<string, string> GetMap { get; }

        public Dictionary<string, string> HeaderMap { get; set; }

        public Dictionary<string, string> PostMap { get; set; }

        public Dictionary<string, string> FileMap { get; }

        public string GetPostString()
        {
            return string.Join("&", PostMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

        public byte[] GetPostBytes()
        {
            return Encoding.GetEncoding(Charset).GetBytes(GetPostString());
        }

        public Stream GetPostStream()
        {
            return _inputStream ?? new MemoryStream(GetPostBytes());
        }

        public void AddHeader(string key, string value)
        {
            HeaderMap[key] = value;
        }

        public void SetBody(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public void AddPostValue(string key, object value)
        {
            PostMap[key] = value?.ToString() ?? string.Empty;
        }

        public void AddPostFile(string key, string file)
        {
            FileMap[key] = file;
        }

        public void AddGetValue(string key, object value)
        {
            GetMap[key] = value?.ToString() ?? string.Empty;
        }

        public string ContentType
        {
            get { return HeaderMap[HttpConstants.ContentType]; }
            set { HeaderMap[HttpConstants.ContentType] = value; }
        }

        public string Referrer
        {
            get { return HeaderMap[HttpConstants.Referrer]; }
            set { HeaderMap[HttpConstants.Referrer] = value; }
        }

        public string UserAgent
        {
            get { return HeaderMap[HttpConstants.UserAgent]; }
            set { HeaderMap[HttpConstants.UserAgent] = value; }
        }

        public string Origin
        {
            get { return HeaderMap[HttpConstants.Origin]; }
            set { HeaderMap[HttpConstants.Origin] = value; }
        }
    }

}
