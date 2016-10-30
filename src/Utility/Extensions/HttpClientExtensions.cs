using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task DownLoadFileAsync(this HttpClient httpClient, string url, string savePath)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            if (File.Exists(savePath)) return;
            using (var file = File.Create(savePath))
            {
                await stream.CopyToAsync(file);
            }
        }

        public static async Task DownLoadFileAsync(this HttpClient httpClient, string url, string saveDir, string fileName)
        {
            await DownLoadFileAsync(httpClient, url, Path.Combine(saveDir, fileName));
        }
    }
}
