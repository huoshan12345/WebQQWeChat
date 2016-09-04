using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Service
{
    public class HttpService : AbstractService, IHttpService
    {
        private CookieContainer _cc;
        private HttpClient _httpClient;

        public string UserAgent { get; set; }

        public void SetHttpProxy(ProxyType proxyType, string proxyHost,
                int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            // TODO ...
        }

        public QQHttpRequest CreateHttpRequest(string method, string url)
        {
            var req = new QQHttpRequest(url, method);
            if (!UserAgent.IsNullOrEmpty()) req.UserAgent = UserAgent;
            return req;
        }

        private static HttpRequestMessage GetHttpRequest(QQHttpRequest qqRequest)
        {
            var request = new HttpRequestMessage(new HttpMethod(qqRequest.Method), qqRequest.Url)
            {
                Headers =
                {
                    Referrer = new Uri(qqRequest.Refer),
                    //UserAgent =
                    //{
                    //    new ProductInfoHeaderValue(qqRequest.UserAgent)
                    //},
                }
            };

            if (qqRequest.Method.Equals(HttpConstants.Post))
            {
                request.Content = new StringContent(qqRequest.GetPostString(), Encoding.GetEncoding(qqRequest.Charset), qqRequest.ContentType);
            }
            else if (qqRequest.Method.Equals(HttpConstants.Get))
            {

            }
            else
            {
                throw new QQException(QQErrorCode.IOError, "not support http method:" + request.Method);
            }
            return request;
        }

        public QQHttpResponse ExecuteHttpRequest(QQHttpRequest qqRequest, IQQHttpListener listener)
        {
            throw new NotSupportedException();
        }

        public async Task<QQHttpResponse> ExecuteHttpRequestAsync(QQHttpRequest request, IQQHttpListener listener, CancellationToken token)
        {
            try
            {
#if DEBUG
                if (request.RawUrl == QQConstants.URL_POLL_MSG)
                {
                    //var str = httpItem.GetRequestHeader();
                    //var count = str.Length;
                }
#endif
                var httpRequest = GetHttpRequest(request);
                var result = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token);

                if (!result.IsSuccessStatusCode) throw new QQException(QQErrorCode.ErrorHttpStatus, result.StatusCode.ToString());

                var response = new QQHttpResponse
                {
                    ResponseMessage = result.ReasonPhrase,
                    ResponseCode = (int)result.StatusCode,
                    Headers = new Dictionary<string, List<string>>(),
                    ResultType = request.ResultType,
                };

                switch (request.ResultType)
                {
                    case ResponseResultType.String:
                    {
                        response.ResponseString = await result.Content.ReadAsStringAsync();
                        break;
                    }
                    case ResponseResultType.Byte:
                    {
                        response.ResponseBytes = await result.Content.ReadAsByteArrayAsync();
                        break;
                    }
                    case ResponseResultType.Stream:
                    {
                        response.ResponseStream = new MemoryStream(await result.Content.ReadAsByteArrayAsync());
                        break;
                    }
                }

                foreach (var header in result.Headers)
                {
                    response.Headers[header.Key] = header.Value.ToList();
                }
                foreach (var header in result.Content.Headers)
                {
                    response.Headers[header.Key] = header.Value.ToList();
                }

                if (result.Headers.Location != null)
                {
                    request.Url = result.Headers.Location.AbsoluteUri;
                    return ExecuteHttpRequest(request, listener);
                }
                else
                {
                    listener.OnHttpHeader(response);
                    listener.OnHttpRead(0, response.GetContentLength());
                    listener.OnHttpFinish(response);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var qqEx = ex as QQException ?? new QQException(ex);
                // throw qqEx;              // 不抛出，而是交给listener处理
                listener.OnHttpError(qqEx); // 这个listener负责推送一个类型为ON_HTTP_ERROR的actor到线程池，这个actor会执行action的OnHttpError方法
                return null;
            }
        }

        public QQHttpCookie GetCookie(string name, string url)
        {
            QQHttpCookie qqHttpCookie = null;
            var cookie = _cc.GetCookies(new Uri(url))[name];
            if (cookie != null) qqHttpCookie = new QQHttpCookie(cookie);
            else Context.Logger.LogError($"获取cookie失败：{name}");
            return qqHttpCookie;
        }

        public override void Init(IQQContext context)
        {
            base.Init(context);
            try
            {
                _cc = new CookieContainer();
                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    CookieContainer = _cc,
                };
                _httpClient = new HttpClient(handler)
                {
                };
                _httpClient.DefaultRequestHeaders.Add(HttpConstants.UserAgent, QQConstants.USER_AGENT);
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.InitError, e);
            }
        }

        public override void Destroy()
        {
            _httpClient.Dispose();
            _httpClient = null;
            _cc = null;
        }

        private string GetMimeType(string file)
        {
            return MimeMapping.GetMimeMapping(file);
        }
    }
}
