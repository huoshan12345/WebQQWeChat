using System;
using System.Collections.Generic;
using System.Text;
using HttpAction.Core;

namespace WebTest
{
    public static class Extensions
    {
        public static HttpRequestItem SetBearerToken(this HttpRequestItem request, string token)
        {
            request.AddHeader("Authorization", "Bearer " + token);
            return request;
        }
    }
}
