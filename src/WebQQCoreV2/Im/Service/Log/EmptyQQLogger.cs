using System;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Service.Log
{
    public class EmptyQQLogger : IQQLogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => Disposable.Empty;

        public IQQContext Context { get; set; }

        public void Init(IQQContext context)
        {
        }

        public void Destroy()
        {
        }
    }
}
