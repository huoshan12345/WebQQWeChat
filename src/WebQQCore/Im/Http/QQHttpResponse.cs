using System.Collections.Generic;
using System.IO;
using System.Text;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Http
{

    /// <summary>
    /// HTTP回复
    /// </summary>
    public class QQHttpResponse
    {
        public const int S_OK = 200;
        public const int S_NOT_MODIFIED = 304;
        public const int S_BAD_REQUEST = 400;
        public const int S_NOT_AUTHORIZED = 401;
        public const int S_FORBIDDEN = 403;
        public const int S_NOT_FOUND = 404;
        public const int S_NOT_ACCEPTABLE = 406;
        public const int S_INTERNAL_SERVER_ERROR = 500;
        public const int S_BAD_GATEWAY = 502;
        public const int S_SERVICE_UNAVAILABLE = 503;

        public int ResponseCode { set; get; }

        public string ResponseMessage { set; get; }

        public Dictionary<string, List<string>> Headers { set; get; }

        public ResponseResultType ResultType { set; get; }

        public string ResponseString { set; get; }

        public byte[] ResponseBytes { set; get; }

        public Stream ResponseStream { set; get; }

        /// <summary>
        /// 返回指定名字的回复头的值 可能有多个返回值时，默认返回第一个值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetHeader(string name)
        {
            List<string> list = null;
            Headers.TryGetValue(name, out list);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 返回指定名字的所有的回复头的值的列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetHeaders(string name)
        {
            return Headers[name];
        }

        /// <summary>
        /// 返回回复内容编码为utf8的字符串
        /// </summary>
        /// <returns></returns>
        public string GetResponseString()
        {
            return ResponseString;
        }

        public long GetContentLength()
        {
            var length = GetHeader(HttpConstants.ContentLength);
            return length != null ? long.Parse(length) : 0;
        }

        public string GetContentType()
        {
            return GetHeader(HttpConstants.ContentType);
        }
    }
}
