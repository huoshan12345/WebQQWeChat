/************************************************************************************
 * 创建人：  huoshan12345
 * 电子邮箱：89009143@qq.com
 * 创建时间：2015/3/12 9:20:09
 * 描述：
/************************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WebQQ.Util
{
    public class Resource
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static Stream LoadEmbededResource(string name)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(p => p.EndsWith(name));
            var stream = resourceName == null ? null : assembly.GetManifestResourceStream(resourceName);
            return stream;
        }

        public static Stream LoadFileResource(string name)
        {
            const string dir = "Resources";
            var path = Path.Combine(dir, name);
            var fs = File.Exists(path) ? File.Open(path, FileMode.Open) : null;
            return fs;
        }

        public static async Task<Stream> LoadServerResourceAsync(string url)
        {
            var stream = await _httpClient.GetStreamAsync(url).ConfigureAwait(false);
            return stream;
        }

        public static T LoadLocalResource<T>(string name, Func<Stream, T> func)
        {
            var resource = LoadFileResource(name) ?? LoadEmbededResource(name);
            using (resource)
            {
                return func(resource);
            }
        }

        public static async Task<T> LoadResourceAsync<T>(string name, string url, Func<Stream, T> func)
        {
            var resource = LoadFileResource(name) ?? LoadEmbededResource(name)
                ?? await LoadServerResourceAsync(url).ConfigureAwait(false);
            using (resource)
            {
                return func(resource);
            }
        }
    }
}
