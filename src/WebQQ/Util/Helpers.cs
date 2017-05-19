using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace WebQQ.Util
{
    public static class Helpers
    {
        public static JsonSerializerSettings JsonCamelSettings { get; } = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public static JsonSerializer JsonCamelSerializer { get; } = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
    }
}
