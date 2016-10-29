using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using System.Net;
using System.Net.Http;
using System.Linq;

namespace HttpActionFrame.Core
{
    public class HttpService : IHttpService, IDisposable
    {
        protected readonly CookieContainer _cc;
        protected readonly HttpClient _httpClient;
        private static readonly string[] NotAddHeaderNames = { HttpConstants.ContentType };

        public HttpService()
        {
            _cc = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = _cc,
            };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add(HttpConstants.UserAgent, HttpConstants.DefaultUserAgent);
        }

        private static HttpRequestMessage GetHttpRequest(HttpRequestItem item)
        {
            var request = new HttpRequestMessage(new HttpMethod(item.Method.ToString()), item.GetUrl());
            switch (item.Method)
            {
                case HttpMethodType.Post:
                    request.Content = new StringContent(item.RawData, item.EncodingType, item.ContentType);
                    break;

                case HttpMethodType.Get:
                case HttpMethodType.Put:
                case HttpMethodType.Delete:
                case HttpMethodType.Head:
                case HttpMethodType.Options:
                case HttpMethodType.Trace:
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
            foreach (var header in response.Content.Headers)
            {
                responseItem.Headers[header.Key] = header.Value.ToList();
            }
        }

        private static async Task ReadContentAsync(HttpResponseMessage response, HttpResponseItem responseItem)
        {
            switch (responseItem.RequestItem.ResultType)
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
        }

        public virtual void SetHttpProxy(ProxyType proxyType, string proxyHost, int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            // TODO
        }

        public virtual async Task<HttpResponseItem> ExecuteHttpRequestAsync(HttpRequestItem requestItem, CancellationToken token, IHttpActionListener actionListener = null)
        {
            var responseItem = new HttpResponseItem() { RequestItem = requestItem };
            try
            {
                var httpRequest = GetHttpRequest(requestItem);
                var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token);
                token.ThrowIfCancellationRequested();
                response.EnsureSuccessStatusCode();
                responseItem.StatusCode = response.StatusCode;
                ReadHeader(response, responseItem);
                token.ThrowIfCancellationRequested();
                actionListener?.OnHttpHeader(responseItem);
                await ReadContentAsync(response, responseItem).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                actionListener?.OnHttpContent(responseItem);
            }
            catch (Exception ex)
            {
                responseItem.Exception = ex;
                actionListener?.OnHttpError(ex);
            }
            return responseItem;
        }

        public virtual Cookie GetCookie(string name, string url)
        {
            return _cc.GetCookies(new Uri(url))[name];
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
