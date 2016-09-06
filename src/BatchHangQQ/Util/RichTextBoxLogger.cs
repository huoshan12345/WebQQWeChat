using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iQQ.Net.BatchHangQQ.Extensions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Log;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.BatchHangQQ.Util
{
    public class RichTextBoxLogger : IQQLogger
    {
        private readonly LogLevel _minLevel;
        private readonly RichTextBox _rtb;

        public RichTextBoxLogger(RichTextBox rtb, LogLevel minLevel = LogLevel.Information)
        {
            _rtb = rtb;
            _minLevel = minLevel;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Task.Run(() =>
            {
                if (!IsEnabled(logLevel)) return;
                if (formatter == null) throw new ArgumentNullException(nameof(formatter));

                var message = formatter(state, exception);
                if (!string.IsNullOrEmpty(message) || exception != null)
                {
                    WriteMessage(logLevel, GetMessage(message, exception));
                }
            });
        }

        public IQQContext Context { get; set; }


        public string GetMessage(string message, Exception exception)
        {
            var qqStr = (Context?.Account.QQ).IsNullOrDefault() ? string.Empty : $"[{Context.Account.QQ}] ";
            return $"{DateTime.Now:HH:mm:ss}> {qqStr}{message}{Environment.NewLine}";
        }

        public bool IsEnabled(string msg, LogLevel level)
        {
            return !msg.IsNullOrEmpty() && IsEnabled(level);
        }

        public void WriteMessage(LogLevel logLevel, string message)
        {
            if (!IsEnabled(message, logLevel)) return;
            var color = GetColor(logLevel);

            // 此处保证是单线程
            _rtb.InvokeIfRequired(() =>
            {
                _rtb.AppendText(message, color);
                _rtb.SelectionStart = _rtb.Text.Length;
                _rtb.ScrollToCaret();
            });
        }

        public Color GetColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical: return Color.DarkRed;
                case LogLevel.Error: return Color.DarkOrchid;
                case LogLevel.Warning: return Color.DarkOrange;
                case LogLevel.Information: return Color.DarkGreen;
                case LogLevel.Debug: return Color.DarkBlue;
                case LogLevel.Trace: return Color.DarkGray;
                default: return Color.Black;
            }
        }
    }
}
