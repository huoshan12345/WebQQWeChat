using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iQQ.Net.WebQQCore.Im.Http
{

    /// <summary>
    /// HTTP回复
    /// </summary>
    public class QQHttpResponse
    {
        /** Constant <code>S_OK=200</code> */
        public const int S_OK = 200;
        /** Constant <code>S_NOT_MODIFIED=304</code> */
        public const int S_NOT_MODIFIED = 304;
        /** Constant <code>S_BAD_REQUEST=400</code> */
        public const int S_BAD_REQUEST = 400;
        /** Constant <code>S_NOT_AUTHORIZED=401</code> */
        public const int S_NOT_AUTHORIZED = 401;
        /** Constant <code>S_FORBIDDEN=403</code> */
        public const int S_FORBIDDEN = 403;
        /** Constant <code>S_NOT_FOUND=404</code> */
        public const int S_NOT_FOUND = 404;
        /** Constant <code>S_NOT_ACCEPTABLE=406</code> */
        public const int S_NOT_ACCEPTABLE = 406;
        /** Constant <code>S_INTERNAL_SERVER_ERROR=500</code> */
        public const int S_INTERNAL_SERVER_ERROR = 500;
        /** Constant <code>S_BAD_GATEWAY=502</code> */
        public const int S_BAD_GATEWAY = 502;
        /** Constant <code>S_SERVICE_UNAVAILABLE=503</code> */
        public const int S_SERVICE_UNAVAILABLE = 503;

        private int _responseCode;// 状态码
        private string _responseMessage;// 状态消息
        private Dictionary<string, List<string>> _headers;//回复头
        private byte[] _responseData;//数据流

        public QQHttpResponse(int responseCode, string responseMessage,
                Dictionary<string, List<string>> headerFields, byte[] responseData)
        {
            this._responseCode = responseCode;
            this._responseMessage = responseMessage;
            this._headers = headerFields;
            this._responseData = responseData;
        }

        public QQHttpResponse()
        {
        }

        public int ResponseCode
        {
            set { _responseCode = value; }
            get { return _responseCode; }
        }

        public string ResponseMessage
        {
            set { _responseMessage = value; }
            get { return _responseMessage; }
        }

        public Dictionary<string, List<string>> Headers
        {
            set { _headers = value; }
            get { return _headers; }
        }

        public byte[] ResponseData
        {
            set { _responseData = value; }
            get { return _responseData; }
        }

        /// <summary>
        /// 返回指定名字的回复头的值 可能有多个返回值时，默认返回第一个值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetHeader(string name)
        {
            List<string> list = null;
            this._headers.TryGetValue(name, out list);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定名字的所有的回复头的值的列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetHeaders(string name)
        {
            return this._headers[name];
        }

        public Stream GetInputStream()
        {
            return new MemoryStream(this._responseData);
        }

        /// <summary>
        /// 获取回复的字符串
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string GetResponseString(string charset)
        {
            return Encoding.GetEncoding(charset).GetString(this._responseData);
        }

        /// <summary>
        /// 返回回复内容编码为utf8的字符串
        /// </summary>
        /// <returns></returns>
        public string GetResponseString()
        {
            return this.GetResponseString("utf-8");
        }

        public override string ToString()
        {
            return "HttpResponse [responseCode=" + _responseCode
                    + ", responseMessage=" + _responseMessage
                    + ", GetResponseString()=" + GetResponseString() + "]";
        }

        public long GetContentLength()
        {
            string length = GetHeader("Content-Length");
            return length != null ? long.Parse(length) : 0;
        }

        public string GetContentType()
        {
            return GetHeader("Content-Type");
        }
    }
}
