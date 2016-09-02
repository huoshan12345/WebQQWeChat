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
        public static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}
