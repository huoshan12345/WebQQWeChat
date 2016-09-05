using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console.Internal;
using Microsoft.Extensions.Logging.Console;

namespace iQQ.Net.WebQQCore.Util
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

    public class SimpleConsoleLogger : ILogger
    {
        private readonly LogLevel _minLevel;
        private readonly object _syncObj = new object();
        public string Name { get; }

        public SimpleConsoleLogger(string name, LogLevel minLevel = LogLevel.Information)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            _minLevel = minLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            return ConsoleLogScope.Push(Name, state);
        }

        public virtual ConsoleColor GetColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical: return ConsoleColor.Red;
                case LogLevel.Error: return ConsoleColor.Magenta;
                case LogLevel.Warning: return ConsoleColor.DarkYellow;
                case LogLevel.Information: return ConsoleColor.DarkGreen;
                case LogLevel.Debug: return ConsoleColor.Gray;
                case LogLevel.Trace: return ConsoleColor.DarkGray;
                default: return ConsoleColor.White;
            }
        }

        public virtual string GetMessage(string msg, Exception exception)
        {
            return $"{DateTime.Now:hh:mm:ss}> {msg}";
        }

        public virtual bool IsEnabled(string msg, LogLevel level)
        {
            return !msg.IsNullOrEmpty() && IsEnabled(level);
        }
        
        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            if (!IsEnabled(message, logLevel)) return;

            var logLevelColor = GetColor(logLevel);
            lock (_syncObj)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = logLevelColor;
                Console.WriteLine(GetMessage(message, exception));
                Console.ForegroundColor = color;
            }
        }
    }
}
