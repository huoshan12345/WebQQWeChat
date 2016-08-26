using System;
using System.Collections.Generic;
using System.IO;
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
                    var buffer = new StringBuilder(_url);
                    buffer.Append("?");

                    foreach (var it in _getMap)
                    {
                        var key = it.Key;
                        var value = it.Value;
                        key = StringHelper.UrlEncode(key);
                        value = StringHelper.UrlEncode(value ?? "");
                        buffer.Append(key);
                        buffer.Append("=");
                        buffer.Append(value);
                        buffer.Append("&");
                    }
                    return buffer.ToString();
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
                    var buffer = new StringBuilder();

                    foreach (var post in PostMap)
                    {
                        if (buffer.Length != 0)
                        {
                            buffer.Append("&");
                        }
                        var key = post.Key;
                        var value = post.Value;
                        key = StringHelper.UrlEncode(key);
                        value = StringHelper.UrlEncode(value ?? "");
                        buffer.Append(key);
                        buffer.Append("=");
                        buffer.Append(value);
                    }
                    return buffer.ToString();
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

        /**
         * 添加请求头
         *
         * @param key a {@link java.lang.String} object.
         * @param value a {@link java.lang.String} object.
         */
        public void AddHeader(string key, string value)
        {
            HeaderMap.Add(key, value, AddChoice.Update);
        }

        /**
         * 以key=&gt;value的方式设置请求体，仅在方法为POST的方式下有用，默认为utf8编码
         *
         * @param keymap a {@link java.util.Map} object.
         */
        public void SetBody(Dictionary<string, string> keymap)
        {
            PostMap = keymap;
        }

        /**
         * 设置请求的数据流
         *
         * @param inputStream a {@link java.io.InputStream} object.
         */
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
    }

}
