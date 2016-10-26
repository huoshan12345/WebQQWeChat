using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im;
using Microsoft.Extensions.Logging;
using Utility.Logger;

namespace iQQ.Net.WebQQCore.Util
{
    internal static class DefaultLogger
    {
        public static ILogger Logger { get; }

        static DefaultLogger()
        {
            var loggerFactory = new LoggerFactory().AddSimpleConsole();
            Logger = loggerFactory.CreateLogger("iQQ.Net");
        }

        private static ILoggerFactory AddSimpleConsole(this ILoggerFactory factory)
        {
            factory.AddProvider(new SimpleConsoleLoggerProvider());
            return factory;
        }
    }
}
