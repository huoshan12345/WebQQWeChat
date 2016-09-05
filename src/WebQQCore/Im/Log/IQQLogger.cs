using iQQ.Net.WebQQCore.Im.Core;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Log
{
    public interface IQQLogger : ILogger
    {
        IQQContext Context { get; set; }
    }
}
