using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Log;

namespace iQQ.Net.WebQQCore.Im.Service
{
    public class HttpService : AbstractService, IHttpService
    {
        private static readonly string[] IgnoreHeaderType = { HttpConstants.UserAgent , HttpConstants.Referer, HttpConstants.ContentType };

        private CookieContainer _cookieContainer;

        public string UserAgent { get; set; }

        public void SetHttpProxy(ProxyType proxyType, string proxyHost,
                int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            // TODO ...
        }

        public QQHttpRequest CreateHttpRequest(string method, string url)
        {
            var req = new QQHttpRequest(url, method);
            req.AddHeader(HttpConstants.UserAgent, UserAgent ?? QQConstants.USER_AGENT);
            return req;
        }

        private HttpItem GetHttpRequest(QQHttpRequest request)
        {
            var uri = new Uri(request.Url);
            var httpItem = new HttpItem()
            {
                KeepAlive = true,
                ProtocolVersion = HttpVersion.Version11,
                ContentType = request.ContentType, // post方法的时候必须填写，要不然服务器无法解析
                Encoding = Encoding.UTF8,
                AllowAutoRedirect = true,
                Method = request.Method,
                Url = uri.AbsoluteUri,
                Host = uri.Host,
                UserAgent = request.UserAgent,
                ReadWriteTimeout = request.ReadTimeout,
                Timeout = request.ConnectTimeout,
                ResultType = ResultType.Byte,
                CookieContainer = _cookieContainer,
            };

            foreach (var header in request.HeaderMap)
            {
                if(IgnoreHeaderType.Contains(header.Key)) continue;
                httpItem.Header.Add(header.Key, header.Value);
            }

            if (request.Method.Equals(HttpConstants.Post))
            {
                if (request.FileMap.Count > 0)
                {
                    httpItem.PostDataType = PostDataType.FilePath;
                    httpItem.Postdata = "";
                }
                else if (request.PostMap.Count > 0)
                {
                    httpItem.PostDataType = PostDataType.String;
                    httpItem.Postdata = request.GetPostString();
                }
                else if (request.PostBody != null)
                {
                    httpItem.ContentType = "application/json; charset=utf-8";
                    httpItem.PostDataType = PostDataType.String;
                    httpItem.Postdata = request.PostBody;
                }
            }
            else if (request.Method.Equals(HttpConstants.Get))
            {

            }
            else
            {
                throw new QQException(QQErrorCode.IO_ERROR, "not support http method:" + request.Method);
            }
            return httpItem;
        }

        public QQHttpResponse ExecuteHttpRequest(QQHttpRequest request, IQQHttpListener listener)
        {
            try
            {
                var httpItem = GetHttpRequest(request);
                var result = new HttpHelper().GetHtml(httpItem);
                var response = new QQHttpResponse
                {
                    ResponseMessage = result.StatusDescription,
                    ResponseCode = (int)result.StatusCode,
                    ResponseData = result.ResultByte,
                    Headers = new Dictionary<string, List<string>>(),
                };

                if (!result.Header.IsNullOrEmpty())
                {
                    foreach (string header in result.Header)
                    {
                        response.Headers.Add(header, result.Header[header]);
                    }
                }

                //if (!result.Cookie.IsNullOrEmpty())
                //{
                //    var cookies = GetCookiesFromHeader(result.Cookie);
                //    foreach (var cookie in cookies)
                //    {
                //        cookie.Domain = result.ResponseUri.Host;
                //        result.CookieCollection.Add(cookie);
                //    }
                //}

                // if (!result.CookieCollection.IsNullOrEmpty()) _cookieContainer.Add(result.CookieCollection);

#if DEBUG
                if (request.Url == QQConstants.URL_CHANNEL_LOGIN)
                {
                    var cookieList = _cookieContainer.GetAllCookies().Select(item => $"name={item.Name}, value={item.Value}, domain={item.Domain}").ToList();
                    var count = cookieList.Count;
                }
#endif



                if (!result.RedirectUrl.IsNullOrEmpty())
                {
                    request.Url = result.RedirectUrl;
                    return ExecuteHttpRequest(request, listener);
                }
                else
                {
                    if (listener != null)
                    {
                        listener.OnHttpHeader(response);
                        listener.OnHttpRead(0, response.GetContentLength());
                        listener.OnHttpFinish(response);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                listener?.OnHttpError(ex);
                return null;
            }
        }

        public Task<QQHttpResponse> ExecuteHttpRequestAsync(QQHttpRequest request, IQQHttpListener listener)
        {
            return Task.Run(() => ExecuteHttpRequest(request, listener));
        }

        private IEnumerable<Cookie> GetCookiesFromHeader(string header)
        {
            return header.Split(';').Select(item => item.Split('=')).Where(item => item.Length == 2)
                .Select(item => new Cookie(item[0].Trim().UrlEncode(), item[1].Trim().UrlEncode()));
        }

        public QQHttpCookie GetCookie(string name, string url)
        {
            // var list = _cookieContainer.GetAllCookies().ToList();
            QQHttpCookie qqHttpCookie = null;
            var cookie = _cookieContainer.GetCookies(new Uri(url))[name] ?? _cookieContainer.GetCookies(name).FirstOrDefault();
            if (cookie != null) qqHttpCookie = new QQHttpCookie(cookie);
            else MyLogger.Default.Error($"获取cookie失败：{name}");
            return qqHttpCookie;
        }

        public override void Init(IQQContext context)
        {
            base.Init(context);
            try
            {
                //                 cookieJar = new QQHttpCookieJar();
                //                 cookieCollection = new CookieCollection();
                _cookieContainer = new CookieContainer();

                //var cookieCollertion = new CookieCollection();
                //const string cookies = "pac_uid=1_89009143; eas_sid=01Z4p6L6k320B463T7K275P4M7; ETK=; superuin=o0089009143; superkey=B1XPeF3uz3WRLMkyBB6Q1VsN*lnmlzB-3*2HstMaaNk_; supertoken=4071070047; ptnick_89009143=e69c88e58589e58f8ce58880; u_89009143=@877pU8znY:1472214012:1472214012:e69c88e58589e58f8ce58880:1; ptcz=1152bd213f139ca8275d31b4ee844b6cd6a62ea2d545f4c278cf97612721d7d7; pt2gguin=o0089009143; uin=o0089009143; skey=@877pU8znY; ptisp=ctc; qv_swfrfh=pc.tgbus.com; qv_swfrfc=v20; qv_swfrfu=http://pc.tgbus.com/gta5/; pgv_info=ssid=s258984257; pgv_pvid=3275166312; o_cookie=89009143; pt_login_sig=BN4*30Tcie7*1GoNT8ndM4DSqxO7m2SX2U4XPnohmjRwWxA7mJV*MX66v1D-V*sJ; pt_clientip=21410ae510ab4ae6; pt_serverip=100a0aab3d2c384b; qrsig=gVnSoCxqYorDzZ4wytcigzOBlckpjAUbn-Uurkd6DG1CsacxbyXpoz0lZTmGl3t8";
                //foreach (var cookie in cookies.Split(';').Select(item => item.Split('=')).Where(item => item.Length == 2).Select(item => new Cookie(item[0].Trim(), item[1].Trim())))
                //{
                //    cookieCollertion.Add(cookie);
                //}
                //_cookieContainer.Add(new Uri("http://ssl.ptlogin2.qq.com/"), cookieCollertion);
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.INIT_ERROR, e);
            }
        }

        public override void Destroy()
        {
            try
            {
                /*
                 * asyncHttpClient.shutdown();
                 */
            }
            catch (ThreadInterruptedException e)
            {
                throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
            }
        }

        private string GetMimeType(string file)
        {
            return MimeMapping.GetMimeMapping(file);
        }

        ////////////////////////////////////////////////////////////////////////
        private const string CANCEL_EX_STRING = "http canceled by user!!!";
        /**
         * <p>checkCanceled.</p>
         *
         * @param isCanceled a bool.
         * @throws java.io.IOException if any.
         */
        public static void CheckCanceled(bool isCanceled)
        {
            if (isCanceled)
            {
                throw new IOException(CANCEL_EX_STRING);
            }
        }

        public void SaveCookie(string fileName = "")
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                var dir = @".\cookie\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fileName = $"{dir}{Context.Account.Username}{".txt"}";
            }
            var fs = File.Create(fileName);
            var sw = new StreamWriter(fs);
            foreach (var cookie in _cookieContainer.GetAllCookies())
            {
                sw.WriteLine("{0};{1};{2};", cookie.Domain, cookie.Name, cookie.Value);
            }
            sw.Close();
            fs.Close();
        }

        public void ReadCookie(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                var dir = @".\cookie\";
                fileName = $"{dir}{Context.Account.Username}{".txt"}";
            }
            if (!File.Exists(fileName))
            {
                return;
            }

            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs);
            var line = "";
            while ((line = sr.ReadLine()) != null)
            {
                var cc = line.Split(';');
                var ck = new Cookie()
                {
                    Discard = false,
                    Expired = false,
                    HttpOnly = false,
                    Domain = cc[0],
                    Name = cc[1],
                    Value = cc[2],
                };
                _cookieContainer.Add(ck);
            }
            sr.Close();
            fs.Close();
        }
    }
}
