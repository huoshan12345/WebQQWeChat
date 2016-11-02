using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetAllMessages(this Exception ex)
        {
            return ex.InnerException != null ? $"{ex.Message}[{GetAllMessages(ex.InnerException)}]" : ex.Message;
        }
    }
}
