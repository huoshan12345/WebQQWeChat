using System;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Service.Log
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
