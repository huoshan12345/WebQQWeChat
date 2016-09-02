using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Service
{
/*
    The following are the allowed log levels (in descending order):

        Fatal
        Error
        Warn
        Info
        Debug
        Trace
        Also to turn off logging, use Off

    Examples when you could use which level:

        Level	Example
        Fatal	Highest level: important stuff down
        Error	For example application crashes / exceptions.
        Warn	Incorrect behavior but the application can continue
        Info	Normal behavior like mail sent, user updated profile etc.
        Debug	Executed queries, user authenticated, session expired
        Trace	Begin method X, end method X etc          
*/


    public interface ILoggerService: IQQService
    {
        void Trace(string message, Exception ex = null);
        void Debug(string message, Exception ex = null);
        void Info(string message, Exception ex = null);
        void Warn(string message, Exception ex = null);
        void Error(string message, Exception ex = null);
        void Fatal(string message, Exception ex = null);
    }
}
