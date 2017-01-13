using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HttpAction.Core;

namespace HttpAction.Service
{
    public interface IHttpService
    {
        void SetHttpProxy(IWebProxy proxy);

        /// <summary>
        /// 执行一个HTTP请求
        /// </summary>
        Task<HttpResponseItem> ExecuteHttpRequestAsync(HttpRequestItem request, CancellationToken token);

        /// <summary>
        /// 获取一个cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Cookie GetCookie(string name, string url);

        CookieCollection GetCookies(string url);
    }
}
