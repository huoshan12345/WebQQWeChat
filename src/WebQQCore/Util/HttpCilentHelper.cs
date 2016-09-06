using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Util
{
    public class HttpRequestItem
    {
        public string RawUrl { get; set; }
        public Encoding EncodingType { get; set; }
        public Dictionary<string, string> HeaderMap { get; }
        public Dictionary<string, string> ValueMap { get; set; }
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
            ValueMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            EncodingType = Encoding.UTF8;
        }

        public string GetPostString()
        {
            return string.Join("&", ValueMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

        public string GetUrl()
        {
            if (Method == HttpMethodType.Get && ValueMap.Count > 0)
            {
                var query = string.Join("&", ValueMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
                return $"{RawUrl}?{query}";
            }
            else
            {
                return RawUrl;
            }
        }
    }

    public class HttpResponseItem
    {
        public ResponseResultType ResultType { set; get; }
        public string ResponseString { set; get; }
        public byte[] ResponseBytes { set; get; }
        public Stream ResponseStream { set; get; }
        public Dictionary<string, List<string>> Headers { set; get; } = new Dictionary<string, List<string>>();
        public Uri Location { set; get; }
        public Exception Exception { set; get; }
        public bool Success => Exception == null && StatusCode == HttpStatusCode.OK;
        public HttpStatusCode StatusCode { set; get; }
    }


    public class HttpCilentHelper
    {
        private static HttpClient _httpClient;
        private CookieContainer _cc;

        public HttpCilentHelper()
        {
            _cc = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = _cc,
            };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add(HttpConstants.UserAgent, QQConstants.USER_AGENT);
        }

        public Task<HttpResponseItem> GetResponseAsync(HttpRequestItem requestItem)
        {
            return GetResponseAsync(requestItem, new CancellationToken());
        }

        public async Task<HttpResponseItem> GetResponseAsync(HttpRequestItem requestItem, CancellationToken token)
        {
            var responseItem = new HttpResponseItem() { ResultType = requestItem.ResultType };
            try
            {
                var httpRequest = GetHttpRequest(requestItem);
                var result = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
                responseItem.StatusCode = result.StatusCode;
                if (!result.IsSuccessStatusCode) throw new WebException($"Unexpected Status Code: {result.StatusCode}");
                responseItem = await GetHttpResponseItem(result, requestItem.ResultType).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                responseItem.Exception = ex;
            }
            return responseItem;
        }

        public async Task<HttpResponseItem> GetResponseAsync(HttpRequestItem requestItem, int retryTimes, CancellationToken token)
        {
            for (var i = 0; i < retryTimes; i++)
            {
                var result = await GetResponseAsync(requestItem, token);
                if (result.Success) return result;
                Thread.Sleep(1000 * i);
            }
            Thread.Sleep(1000 * retryTimes);
            return await GetResponseAsync(requestItem, token);
        }

        public async Task<HttpResponseItem> GetAsync(string url)
        {
            return await GetResponseAsync(new HttpRequestItem(url, HttpMethodType.Get));
        }

        public async Task<HttpResponseItem> GetAsync(string url, int retryTimes)
        {
            for (var i = 0; i < retryTimes; i++)
            {
                var result = await GetAsync(url);
                if (result.Success) return result;
                Thread.Sleep(1000 * i);
            }
            Thread.Sleep(1000 * retryTimes);
            return await GetAsync(url);
        }

        private static HttpRequestMessage GetHttpRequest(HttpRequestItem item)
        {
            var request = new HttpRequestMessage(new HttpMethod(item.Method.ToString()), item.GetUrl());
            if (!item.Referrer.IsNullOrEmpty())
            {
                request.Headers.Add(HttpConstants.Referrer, item.Referrer);
            }

            switch (item.Method)
            {
                case HttpMethodType.Post:
                request.Content = new StringContent(item.GetPostString(), item.EncodingType, item.ContentType);
                break;
                case HttpMethodType.Get:
                break;
                default:
                break;
            }
            return request;
        }

        private static async Task<HttpResponseItem> GetHttpResponseItem(HttpResponseMessage response, ResponseResultType resultType)
        {
            var responseItem = new HttpResponseItem
            {
                ResultType = resultType,
                Location = response.Headers.Location,
                StatusCode = response.StatusCode
            };
            switch (resultType)
            {
                case ResponseResultType.String:
                {
                    responseItem.ResponseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    break;
                }
                case ResponseResultType.Byte:
                {
                    responseItem.ResponseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    break;
                }
                case ResponseResultType.Stream:
                {
                    responseItem.ResponseStream = new MemoryStream(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
                    break;
                }
            }
            foreach (var header in response.Headers)
            {
                responseItem.Headers[header.Key] = header.Value.ToList();
            }
            foreach (var header in response.Content.Headers)
            {
                responseItem.Headers[header.Key] = header.Value.ToList();
            }
            return responseItem;
        }

    }
}
