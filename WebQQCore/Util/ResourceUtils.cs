/************************************************************************************
 * 创建人：  huoshan12345
 * 电子邮箱：89009143@qq.com
 * 创建时间：2015/3/12 9:20:09
 * 描述：
/************************************************************************************/

using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace iQQ.Net.WebQQCore.Util
{
    public class ResourceUtils
    {
        public static string LoadResource(string name)
        {
            string jsName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(p => p.EndsWith(name));
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream(jsName);
            StreamReader sr = new StreamReader(sm);
            string source = sr.ReadToEnd();
            sr.Close();
            return source;
        }

        public static string LoadResourceFromServer(string url)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        public static string LoadResource(string localName, string url)
        {
            try
            {
                return LoadResource(localName);
            }
            catch { }

            try
            {
                return LoadResourceFromServer(url);
            }
            catch { }

            return "";
        }
    }
}
