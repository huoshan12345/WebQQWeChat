/************************************************************************************
 * 创建人：  huoshan12345
 * 电子邮箱：89009143@qq.com
 * 创建时间：2015/3/12 9:20:09
 * 描述：
/************************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using iQQ.Net.WebQQCore.Util.Log;

namespace iQQ.Net.WebQQCore.Util
{
    public class Resource
    {
        public static Stream LoadLocalResource(string name)
        {
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(p => p.EndsWith(name));
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            return stream;
        }

        public static Stream LoadServerResource(string url)
        {
            var client = new WebClient();
            var stream = client.OpenRead(url);
            return stream;
        }

        public static T LoadLocalResource<T>(string name, Func<Stream, T> func)
        {
            using (var resource = LoadLocalResource(name))
            {
                return func(resource);
            }
        }

        public static T LoadServerResource<T>(string url, Func<Stream, T> func)
        {
            using (var resource = LoadServerResource(url))
            {
                return func(resource);
            }
        }

        public static T LoadResource<T>(string name, string url, Func<Stream, T> func)
        {
            try
            {
                var resource = LoadLocalResource(url);
                return func(resource);
            }
            catch (Exception ex)
            {
                MyLogger.Default.Warn($"从本地加载资源失败：{name}", ex);
            }

            try
            {
                var resource = LoadServerResource(url);
                return func(resource);
            }
            catch (Exception ex)
            {
                MyLogger.Default.Warn($"从服务器加载资源失败：{url}", ex);
            }

            return default(T);
        }
    }
}
