using System;
using System.Linq;
using System.Net;
using System.Text;
using Utility.Extensions;
using Utility.HttpAction.Action;
using Utility.HttpAction.Core;

namespace Utility.HttpAction
{
    public static class Extensions
    {
        public static string GetRequestHeader(this HttpRequestItem request, CookieCollection cookieCollection)
        {
            var sb = new StringBuilder();
            sb.AppendLineIf($"{HttpConstants.Referrer}: { request.Referrer}", !request.Referrer.IsNullOrEmpty());
            sb.AppendLineIf($"{HttpConstants.ContentType}: {request.ContentType}", !request.ContentType.IsNullOrEmpty());
            var cookies = cookieCollection.OfType<Cookie>();
            sb.AppendLine($"{HttpConstants.Cookie}: {string.Join("; ", cookies)}");
            return sb.ToString();
        }

        public static string GetRequestHeader(this HttpRequestItem request, CookieContainer cookieContainer)
        {
            return GetRequestHeader(request, cookieContainer.GetCookies(new Uri(request.RawUrl)));
        }

        public static IAction CreateAction<T>(this IActionFactory factory, params object[] args) where T : IAction
        {
            return factory.CreateAction(typeof(T), args);
        }
    }
}
