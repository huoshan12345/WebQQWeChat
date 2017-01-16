using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;

namespace HttpAction.Service
{
    public class HttpService : IHttpService, IDisposable
    {
        protected readonly CookieContainer _cookieContainer;
        protected HttpClient _httpClient;
        private static readonly string[] NotAddHeaderNames = { HttpConstants.ContentType, HttpConstants.Cookie };

        public HttpService(IWebProxy proxy = null)
        {
            _cookieContainer = new CookieContainer();
            _httpClient = CreateHttpClient(proxy);
        }

        private HttpClient CreateHttpClient(IWebProxy proxy)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = _cookieContainer,
                UseCookies = true
            };
            if (proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = proxy;
            }
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
            _httpClient.DefaultRequestHeaders.Add(HttpConstants.UserAgent, HttpConstants.DefaultUserAgent);
            return _httpClient;
        }

        private HttpRequestMessage GetHttpRequest(HttpRequestItem item)
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
            var cookies = item.HeaderMap.GetOrDefault(HttpConstants.Cookie);
            if (!cookies.IsNullOrEmpty())
            {
                _cookieContainer.SetCookies(request.RequestUri, cookies);
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

        public virtual void SetHttpProxy(IWebProxy proxy)
        {
            var client = CreateHttpClient(proxy);
            var oldClient = _httpClient;
            lock (_httpClient)
            {
                _httpClient = client;
            }
            oldClient.Dispose();
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
