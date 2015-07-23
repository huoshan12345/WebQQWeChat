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
        private CookieContainer _cookieContainer;

        private string _userAgent;
        public string UserAgent
        {
            set { _userAgent = value; }
        }

        public void SetHttpProxy(ProxyType proxyType, string proxyHost,
                int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            // TODO ...
        }

        public QQHttpRequest CreateHttpRequest(string method, string url)
        {
            QQHttpRequest req = new QQHttpRequest(url, method);
            req.AddHeader("User-Agent", _userAgent ?? QQConstants.USER_AGENT);
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public Task<QQHttpResponse> ExecuteHttpRequest(QQHttpRequest request, IQQHttpListener listener)
        {
            return Task.Run(() =>
            {
                try
                {
                    var httpItem = new HttpItem()
                    {
                        KeepAlive = true,
                        ProtocolVersion = HttpVersion.Version10,
                        ContentType = "application/x-www-form-urlencoded; charset=UTF-8", // post方法的时候必须填写，要不然服务器无法解析
                        Encoding = Encoding.UTF8,
                        Allowautoredirect = true,
                        Method = request.Method,
                        URL = request.Url,
                        ReadWriteTimeout = (request.ReadTimeout > 0) ? request.ReadTimeout : 100000,
                        Timeout = (request.ConnectTimeout > 0) ? request.ConnectTimeout : 30000,
                        ResultType = ResultType.Byte,
                    };

                    if (request.HeaderMap.ContainsKey("User-Agent"))
                    {
                        httpItem.UserAgent = request.HeaderMap["User-Agent"];
                        request.HeaderMap.Remove("User-Agent");
                    }
                    if (request.HeaderMap.ContainsKey("Referer"))
                    {
                        httpItem.Referer = request.HeaderMap["Referer"];
                        request.HeaderMap.Remove("Referer");
                    }

                    foreach (var header in request.HeaderMap)
                    {
                        httpItem.Header.Add(header.Key, header.Value);
                    }
                    httpItem.CookieContainer = _cookieContainer;

                    if (request.Method.Equals("POST"))
                    {
                        if (request.FileMap.Count > 0)
                        {
                            httpItem.PostDataType = PostDataType.FilePath;
                            httpItem.Postdata = "";
                        }
                        else if (request.PostMap.Count > 0)
                        {
                            httpItem.PostDataType = PostDataType.String;
                            httpItem.Postdata = request.InputString;
                        }
                    }
                    else if (request.Method.Equals("GET"))
                    {

                    }
                    else
                    {
                        throw new QQException(QQErrorCode.IO_ERROR, "not support http method:" + request.Method);
                    }

                    HttpResult result = new HttpHelper().GetHtml(httpItem);
                    QQHttpResponse response = new QQHttpResponse()
                    {
                        ResponseMessage = result.StatusDescription,
                        ResponseCode = (int)result.StatusCode,
                        ResponseData = result.ResultByte,
                        Headers = new Dictionary<string, List<string>>(),
                    };
                    if (result.Header != null)
                    {
                        foreach (string header in result.Header)
                        {
                            response.Headers.Add(header, result.Header[header]);
                        }
                    }
                    if (result.CookieCollection != null)
                    {
                        _cookieContainer.Add(result.CookieCollection);
                    }

                    if (listener != null)
                    {
                        listener.OnHttpHeader(response);
                        listener.OnHttpRead(0, response.GetContentLength());
                        listener.OnHttpFinish(response);
                    }
                    return response;
                }
                catch (IOException e)
                {
                    throw new QQException(QQErrorCode.IO_ERROR, e);
                }
                catch (Exception e)
                {
                    throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
                }
            });
        }

        public QQHttpCookie GetCookie(string name, string url)
        {
            var list = _cookieContainer.GetAllCookies().ToList();
            QQHttpCookie qqHttpCookie = null;
            Cookie cookie = _cookieContainer.GetCookies(new Uri(url))[name] ?? _cookieContainer.GetCookies(name).FirstOrDefault();
            if (cookie != null) qqHttpCookie = new QQHttpCookie(cookie);
            else MyLogger.Default.Error($"获取cookie失败：{name}");
            return qqHttpCookie;
        }

        public override void Init(QQContext context)
        {
            base.Init(context);
            try
            {
                //                 cookieJar = new QQHttpCookieJar();
                //                 cookieCollection = new CookieCollection();
                _cookieContainer = new CookieContainer();
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
                string dir = @".\cookie\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fileName = string.Format("{0}{1}{2}", dir, Context.Account.Username, ".txt");
            }
            FileStream fs = File.Create(fileName);
            StreamWriter sw = new StreamWriter(fs);
            foreach (Cookie cookie in _cookieContainer.GetAllCookies())
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
                string dir = @".\cookie\";
                fileName = string.Format("{0}{1}{2}", dir, Context.Account.Username, ".txt");
            }
            if (!File.Exists(fileName))
            {
                return;
            }

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] cc = line.Split(';');
                Cookie ck = new Cookie()
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
