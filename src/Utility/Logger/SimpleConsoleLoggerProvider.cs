using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Utility.Logger
{
    public class SimpleConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel _minLevel;
        private readonly ConcurrentDictionary<string, SimpleConsoleLogger> _loggers = new ConcurrentDictionary<string, SimpleConsoleLogger>();

        public SimpleConsoleLoggerProvider(LogLevel minLevel = LogLevel.Information)
        {
            _minLevel = minLevel;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (name) => new SimpleConsoleLogger(name, _minLevel));
        }
    }
}
