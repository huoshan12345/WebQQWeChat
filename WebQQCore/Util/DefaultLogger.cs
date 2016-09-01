using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace iQQ.Net.WebQQCore.Util
{
    internal static class DefaultLogger
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Debug(string msg, params object[] args)
        {
            _logger.Debug(msg, args);
        }

        public static void Debug(string msg, Exception err)
        {
            _logger.Debug(err, msg);
        }

        public static void Info(string msg, params object[] args)
        {
            _logger.Info(msg, args);
        }

        public static void Info(string msg, Exception err)
        {
            _logger.Info(err, msg);
        }

        public static void Trace(string msg, params object[] args)
        {
            _logger.Trace(msg, args);
        }

        public static void Trace(string msg, Exception err)
        {
            _logger.Trace(err, msg);
        }

        public static void Error(string msg, params object[] args)
        {
            _logger.Error(msg, args);
        }

        public static void Error(string msg, Exception err)
        {
            _logger.Error(err, msg);
        }

        public static void Fatal(string msg, params object[] args)
        {
            _logger.Fatal(msg, args);
        }

        public static void Fatal(string msg, Exception err)
        {
            _logger.Fatal(err, msg);
        }

        public static void Warn(string msg, params object[] args)
        {
            _logger.Warn(msg, args);
        }

        public static void Warn(string msg, Exception err)
        {
            _logger.Warn(err, msg);
        }
    }
}
