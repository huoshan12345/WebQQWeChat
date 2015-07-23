using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iQQ.Net.WebQQCore.Im.Http
{
    /// <summary>
    /// 保存和读取cookie
    /// </summary>
    public class QQHttpCookieJar
    {
        /**
         * <p>Constructor for QQHttpCookieJar.</p>
         */
        public QQHttpCookieJar()
        {
            this.CookieContainer = new List<QQHttpCookie>();
        }

        public List<QQHttpCookie> CookieContainer { get; set; }

        /**
         * <p>GetCookie.</p>
         *
         * @param name a {@link java.lang.String} object.
         * @param url a {@link java.lang.String} object.
         * @return a {@link iqq.im.http.QQHttpCookie} object.
         */
        public QQHttpCookie GetCookie(string name, string url)
        {
            return CookieContainer.FirstOrDefault(cookie => cookie.Name.Equals(name));
        }

        /**
         * <p>updateCookies.</p>
         *
         * @param tmpCookies a {@link java.util.List} object.
         */
        public void UpdateCookies(List<string> tmpCookies)
        {
            List<string> newCookies = new List<string>();
            if (tmpCookies != null)
            {
                newCookies.AddRange(tmpCookies);
            }

            if (newCookies.Count > 0)
            {
                foreach (var it in newCookies)
                {
                    QQHttpCookie cookie = new QQHttpCookie(it);
                    QQHttpCookie oldCookie = this.GetCookie(cookie.Name, null);
                    //如果有之前相同名字的Cookie,删除之前的cookie
                    if (oldCookie != null)
                    {
                        CookieContainer.Remove(oldCookie);
                        //如果新cookie的值不为空，就添加到新的cookie到列表中
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            CookieContainer.Add(cookie);
                        }
                    }
                    else
                    {
                        CookieContainer.Add(cookie);
                    }
                }
            }
        }

        public string GetCookieHeader(string url)
        {
            Uri u = new Uri(url);
            StringBuilder buffer = new StringBuilder();


            foreach (var cookie in CookieContainer)
            {
                if (cookie.Expired != default(DateTime) && cookie.Expired < DateTime.Now)
                {
                    //已经过期，删除
                }
                else if (cookie.Path.StartsWith(cookie.Path))
                {
                    buffer.Append(cookie.Name);
                    buffer.Append("=");
                    buffer.Append(cookie.Value);
                    buffer.Append("; ");
                }
            }
            return buffer.ToString();
        }
    }

}
