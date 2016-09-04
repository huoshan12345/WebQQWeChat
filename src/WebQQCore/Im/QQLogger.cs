using System;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace iQQ.Net.WebQQCore.Im
{
    public class QQLogger : SimpleConsoleLogger
    {
        public QQLogger(IQQClient qq) : base(qq.GetHashCode().ToString(), formatter: (s, e) =>
        {
            var qqStr = qq.Account.QQ.IsDefault() ? string.Empty : $"[{qq.Account.QQ}] ";
            return $"{DateTime.Now:hh:mm:ss}> {qqStr}{s}";
        })
        {
        }

    }
}
