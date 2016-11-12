using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpAction.Core;

namespace HttpAction.Service
{
    public class HttpService : IHttpService, IDisposable
    {
        protected readonly CookieContainer _cookieContainer;
        protected readonly HttpClient _httpClient;
        private static readonly string[] NotAddHeaderNames = { HttpConstants.ContentType };

        public HttpService()
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = _cookieContainer,
            };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add(HttpConstants.UserAgent, HttpConstants.DefaultUserAgent);
        }

        private static HttpRequestMessage GetHttpRequest(HttpRequestItem item)
        {
            var request = new HttpRequestMessage(new HttpMethod(item.Method.ToString().ToUpper()), item.GetUrl());
            switch (item.Method)
            {
                case HttpMethodType.Post:
                case HttpMethodType.Put:
                case HttpMethodType.Delete:
                case HttpMethodType.Head:
                case HttpMethodType.Options:
                case HttpMethodType.Trace:
                    request.Content = new StringContent(item.RawData, item.EncodingType, item.ContentType);
                    break;

                case HttpMethodType.Get:
                default:
                    break;
            }
            foreach (var header in item.HeaderMap.Where(h => !NotAddHeaderNames.Contains(h.Key)))
            {
                request.Headers.Add(header.Key, header.Value);
            }
            return request;
        }

        private static void ReadHeader(HttpResponseMessage response, HttpResponseItem responseItem)
        {
            foreach (var header in response.Headers)
            {
                responseItem.Headers[header.Key] = header.Value.ToList();
            }
        }

        private static async Task ReadContentAsync(HttpResponseMessage response, HttpResponseItem responseItem)
        {
            switch (responseItem.RequestItem.ResultType)
            {
                case HttpResultType.String:
                    responseItem.ResponseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    break;

                case HttpResultType.Byte:
                    responseItem.ResponseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    break;

                case HttpResultType.Stream:
                    responseItem.ResponseStream = new MemoryStream(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
                    break;
            }
            foreach (var header in response.Content.Headers)
            {
                responseItem.Headers[header.Key] = header.Value.ToList();
            }
        }

        public virtual void SetHttpProxy(ProxyType proxyType, string proxyHost, int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            // TODO
        }

        public virtual async Task<HttpResponseItem> ExecuteHttpRequestAsync(HttpRequestItem requestItem, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var responseItem = new HttpResponseItem { RequestItem = requestItem };
            var httpRequest = GetHttpRequest(requestItem);
            using (var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                responseItem.StatusCode = response.StatusCode;
                ReadHeader(response, responseItem);
                await ReadContentAsync(response, responseItem).ConfigureAwait(false);
                return responseItem;
            }
        }

        public virtual Cookie GetCookie(string name, string url)
        {
            return _cookieContainer.GetCookies(new Uri(url))[name];
        }

        public CookieCollection GetCookies(string url)
        {
            return _cookieContainer.GetCookies(new Uri(url));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
