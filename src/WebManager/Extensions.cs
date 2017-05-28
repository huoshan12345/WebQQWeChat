using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebManager
{
    public static class Extensions
    {
        public static bool IsApiUrl(this PathString path)
        {
            return path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsApiUrl(this HttpContext context)
        {
            return IsApiUrl(context.Request.Path);
        }

        public static bool IsApiUrl(this HttpRequest request)
        {
            return IsApiUrl(request.Path);
        }
    }
}
