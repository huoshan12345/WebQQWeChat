using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iQQ.Net.WebQQCore.Im.Http
{
    /// <summary>
    /// 保存和读取cookie
    /// </summary>
    [Obsolete("目前用不上")]
    public class QQHttpCookieJar
    {
        public QQHttpCookieJar()
        {
            this.CookieContainer = new List<QQHttpCookie>();
        }

        public List<QQHttpCookie> CookieContainer { get; set; }
        
        public QQHttpCookie GetCookie(string name, string url)
        {
            return CookieContainer.FirstOrDefault(cookie => cookie.Name.Equals(name));
        }

        public void UpdateCookies(List<string> tmpCookies)
        {
            var newCookies = new List<string>();
            if (tmpCookies != null)
            {
                newCookies.AddRange(tmpCookies);
            }

            if (newCookies.Count > 0)
            {
                foreach (var it in newCookies)
                {
                    var cookie = new QQHttpCookie(it);
                    var oldCookie = this.GetCookie(cookie.Name, null);
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
            var u = new Uri(url);
            var buffer = new StringBuilder();


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
