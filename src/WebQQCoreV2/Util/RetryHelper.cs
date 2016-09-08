using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util
{
    public static class RetryHelper
    {
        public static T Retry<T>(Func<T> func, int retryTimes, TimeSpan ts)
        {
            var exceptions = new List<Exception>();
            for (var i = 0; i < retryTimes; i++)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    Thread.Sleep(ts);
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
