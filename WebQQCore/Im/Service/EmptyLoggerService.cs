using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Service
{
    public class EmptyLoggerService : ILoggerService
    {
        public void Init(IQQContext context)
        {
        }

        public void Destroy()
        {
        }

        public void Trace(string message, Exception ex = null)
        {
        }

        public void Debug(string message, Exception ex = null)
        {
        }

        public void Info(string message, Exception ex = null)
        {
        }

        public void Warn(string message, Exception ex = null)
        {
        }

        public void Error(string message, Exception ex = null)
        {
        }

        public void Fatal(string message, Exception ex = null)
        {
        }
    }
}
