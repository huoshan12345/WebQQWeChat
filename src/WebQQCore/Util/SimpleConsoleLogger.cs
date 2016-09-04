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
    internal static class DefaultLoggerParameter
    {
        internal static readonly Func<string, Exception, string> DefaultFormatter = ((s, e) => $"{DateTime.Now:hh:mm:ss}> {s}");
        internal static readonly Func<string, LogLevel, bool> DefaultFilter = ((category, logLevel) => !category.IsNullOrEmpty() && logLevel >= LogLevel.Information);
        internal static readonly Func<LogLevel, ConsoleColor> DefaultColor = GetLogLevelConsoleColor;

        internal static ConsoleColor GetLogLevelConsoleColor(LogLevel logLevel)
        {
            // We must explicitly set the background color if we are setting the foreground color,
            // since just setting one can look bad on the users console.
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
    }

    public class SimpleConsoleLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly Func<string, Exception, string> _formatter;
        private readonly Func<LogLevel, ConsoleColor> _color;

        private readonly ConcurrentDictionary<string, SimpleConsoleLogger> _loggers = new ConcurrentDictionary<string, SimpleConsoleLogger>();

        public void Dispose()
        {
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (name) => new SimpleConsoleLogger(name, _filter, _formatter, _color));
        }

        public SimpleConsoleLoggerProvider(Func<string, LogLevel, bool> filter = null, Func<string, Exception, string> formatter = null, Func<LogLevel, ConsoleColor> color = null)
        {
            _color = color;
            _filter = filter ?? DefaultLoggerParameter.DefaultFilter;
            _formatter = formatter ?? DefaultLoggerParameter.DefaultFormatter;
            _color = color ?? DefaultLoggerParameter.DefaultColor;
        }
    }

    public class SimpleConsoleLogger : ILogger
    {
        private readonly object _syncObj = new object();
        public virtual Func<string, LogLevel, bool> Filter { get; }
        public virtual Func<string, Exception, string> Formatter { get; }
        public virtual Func<LogLevel, ConsoleColor> Color { get; }

        public SimpleConsoleLogger(string name, Func<string, LogLevel, bool> filter = null, Func<string, Exception, string> formatter = null, Func<LogLevel, ConsoleColor> color = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            Filter = filter ?? DefaultLoggerParameter.DefaultFilter;
            Formatter = formatter ?? DefaultLoggerParameter.DefaultFormatter;
            Color = color ?? DefaultLoggerParameter.DefaultColor;
        }

        public string Name { get; }

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

        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            if (!Filter(message, logLevel) || string.IsNullOrEmpty(message)) return;

            var logLevelColor = Color(logLevel);
            lock (_syncObj)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = logLevelColor;
                Console.WriteLine(Formatter(message, exception));
                Console.ForegroundColor = color;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Filter(Name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            return ConsoleLogScope.Push(Name, state);
        }
    }
}
