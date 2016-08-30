using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Http
{
    /**
     *
     * HTTP请求
     *
     * @author solosky
     */
    public class QQHttpRequest
    {
        private string _url;

        private readonly Dictionary<string, string> _getMap;

        private Stream _inputStream;

        private byte[] _inputBytes;

        private string _inputString;

        private string _charset;


        public QQHttpRequest(string url, string method)
        {
            _url = url;
            Method = method;
            HeaderMap = new Dictionary<string, string>();
            PostMap = new Dictionary<string, string>();
            _getMap = new Dictionary<string, string>();
            FileMap = new Dictionary<string, string>();
        }

        public string Url
        {
            get
            {
                if (_getMap.Count > 0)
                {
                    //var buffer = new StringBuilder(_url);
                    //buffer.Append("?");
                    //foreach (var it in _getMap)
                    //{
                    //    var key = it.Key.UrlEncode();
                    //    var value = it.Value.UrlEncode();
                    //    buffer.Append(key);
                    //    buffer.Append("=");
                    //    buffer.Append(value);
                    //    buffer.Append("&");
                    //}
                    //return buffer.ToString();
                    var query = string.Join("&", _getMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
                    return $"{_url}?{query}";
                }
                else
                {
                    return _url;
                }
            }
            set { _url = value; }
        }

        public string Method { get; set; }

        public int Timeout { get; set; }


        public string PostBody { get; set; }

        public Dictionary<string, string> HeaderMap { get; set; }

        public Stream InputStream
        {
            get
            {
                if (_inputStream != null)
                {
                    return _inputStream;
                }
                else if (PostMap.Count > 0)
                {
                    return new MemoryStream(Encoding.GetEncoding(_charset).GetBytes(InputString));
                }
                else
                {
                    return null;
                }
            }

            set { _inputStream = value; }
        }

        public Stream OutputStream { get; set; }

        public string Charset
        {
            get { return _charset ?? "utf-8"; }
            set { _charset = value; }
        }

        public int ConnectTimeout { get; set; }

        public int ReadTimeout { get; set; }

        public Dictionary<string, string> PostMap { get; private set; }

        public Dictionary<string, string> FileMap { get; }

        public string InputString
        {
            get
            {
                if (_inputString != null)
                {
                    return _inputString;
                }
                else if (PostMap.Count > 0)
                {
                    //AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    //var buffer = new StringBuilder();

                    //foreach (var post in PostMap)
                    //{
                    //    if (buffer.Length != 0)
                    //    {
                    //        buffer.Append("&");
                    //    }
                    //    var key = post.Key;
                    //    var value = post.Value;
                    //    key = key.UrlEncode();
                    //    value = (value ?? "").UrlEncode();
                    //    buffer.Append(key);
                    //    buffer.Append("=");
                    //    buffer.Append(value);
                    //}
                    //return buffer.ToString();
                    return string.Join("&", PostMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
                }
                else
                {
                    return null;
                }
            }
            set { _inputString = value; }
        }

        public byte[] InputBytes
        {
            get
            {
                if (_inputBytes != null)
                {
                    return _inputBytes;
                }
                else if (PostMap.Count > 0)
                {
                    return Encoding.GetEncoding(_charset).GetBytes(InputString);
                }
                else
                {
                    return null;
                }
            }
            set { _inputBytes = value; }
        }

        public void AddHeader(string key, string value)
        {
            HeaderMap.Add(key, value, AddChoice.Update);
        }

        public void SetBody(Dictionary<string, string> keymap)
        {
            PostMap = keymap;
        }

        public void SetBody(Stream inputStream)
        {
            _inputStream = inputStream;
        }


        public void AddPostValue(string key, string value)
        {
            PostMap.Add(key, value ?? string.Empty, AddChoice.Update);
        }

        public void AddPostValue(string key, object value)
        {
            AddPostValue(key, value?.ToString());
        }

        public void AddPostFile(string key, string file)
        {
            FileMap.Add(key, file, AddChoice.Update);
        }

        public void AddGetValue(string key, string value)
        {
            _getMap.Add(key, value ?? string.Empty, AddChoice.Update);
        }

        public void AddGetValue(string key, object value)
        {
            AddGetValue(key, value?.ToString());
        }

        public void AddRefer(string refer)
        {
            AddHeader(HttpConstants.Referer, refer);
        }
    }

}
