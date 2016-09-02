using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;
using NLog;

namespace iQQ.Net.WebQQCore.Im.Service
{
    public class NLoggerService: ILoggerService
    {
        private Logger _logger;
        private IQQContext _context;
        private string LoggerPrefix => _context.Account.QQ == 0 ? string.Empty : $"[{_context.Account.QQ}]: ";

        public void Init(IQQContext context)
        {
            _logger = DefaultLogger.Logger;
            _context = context;
        }

        public void Destroy()
        {
            _logger = null;
        }

        public void Trace(string message, Exception ex = null)
        {
            _logger.Trace(ex, $"{LoggerPrefix}{message}");
        }

        public void Debug(string message, Exception ex = null)
        {
            _logger.Debug(ex, $"{LoggerPrefix}{message}");
        }

        public void Info(string message, Exception ex = null)
        {
            _logger.Info(ex, $"{LoggerPrefix}{message}");
        }

        public void Warn(string message, Exception ex = null)
        {
            _logger.Warn(ex, $"{LoggerPrefix}{message}");
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(ex, $"{LoggerPrefix}{message}");
        }

        public void Fatal(string message, Exception ex = null)
        {
            _logger.Fatal(ex, $"{LoggerPrefix}{message}");
        }
    }
}
