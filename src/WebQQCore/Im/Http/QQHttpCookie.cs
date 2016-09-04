using System;
using System.Net;

namespace iQQ.Net.WebQQCore.Im.Http
{
    public class QQHttpCookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Expired { get; set; }

        public QQHttpCookie(string name, string value, string domain, string path,
                DateTime expired)
        {
            Name = name;
            Value = value;
            Domain = domain;
            Path = path;
            Expired = expired;
        }

        public QQHttpCookie(Cookie cookie)
        {
            Name = cookie.Name;
            Value = cookie.Value;
            Domain = cookie.Domain;
            Path = cookie.Path;
            Expired = cookie.Expires;
        }

        /// <summary>
        /// 通过一个原始的cookie字符串解析cookie
        /// </summary>
        /// <param name="cookie"></param>
        public QQHttpCookie(string cookie)
        {
            /*
            这里只解析name, value, domain, path, Max-Age(expired)
            The syntax for the Set-Cookie response header is

               set-cookie      =       "Set-Cookie:" cookies
               cookies         =       1#cookie
               cookie          =       NAME "=" VALUE *(";" cookie-av)
               NAME            =       attr
               VALUE           =       value
               cookie-av       =       "Comment" "=" value
                               |       "Domain" "=" value
                               |       "Max-Age" "=" value
                               |       "Path" "=" value
                               |       "Secure"
                               |       "Version" "=" 1*DIGIT
            */
            var parts = cookie.Split(';');
            if (parts.Length < 2) throw new ArgumentException("Invalid cookie string:" + cookie);

            for (var i = 0; i < parts.Length; i++)
            {
                var pairs = parts[i].Split('=');
                var key = pairs[0].Trim();
                var val = pairs.Length > 1 ? pairs[1].Trim() : "";
                if (i == 0)
                {	//解析name和value
                    Name = key;
                    Value = val;
                }
                else
                {
                    key = key.ToLower();
                    if (key.Equals("domain"))
                    {
                        Domain = val;
                    }
                    else if (key.Equals("max-age"))
                    {
                        try
                        {
                            Expired = DateTime.Parse(val);
                        }
                        catch (FormatException e)
                        {
                            throw new ArgumentException("parse exipred failed.", e);
                        }
                    }
                    else if (key.Equals("path"))
                    {
                        Path = val;
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Cookie [name=" + Name + ", value=" + Value + ", domain="
                    + Domain + ", path=" + Path + ", expired=" + Expired + "]";
        }
    }

}
