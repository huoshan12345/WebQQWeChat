using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Action;
using Utility.HttpAction.Core;

namespace Utility.HttpAction.Service
{
    public interface IHttpService
    {
        /// <summary>
        /// 设置HTTP代理
        /// </summary>
        /// <param name="proxyType">代理类型</param>
        /// <param name="proxyHost">代理主机</param>
        /// <param name="proxyPort">代理端口</param>
        /// <param name="proxyAuthUser">认证用户名， 如果不需要认证，设置为null</param>
        /// <param name="proxyAuthPassword">认证密码，如果不需要认证，设置为null</param>
        void SetHttpProxy(ProxyType proxyType, string proxyHost, int proxyPort, string proxyAuthUser, string proxyAuthPassword);

        /// <summary>
        /// 执行一个HTTP请求
        /// </summary>
        Task<HttpResponseItem> ExecuteHttpRequestAsync(HttpRequestItem request, CancellationToken token, IHttpActionListener actionListener = null);

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
