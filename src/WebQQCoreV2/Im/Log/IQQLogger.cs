using iQQ.Net.WebQQCore.Im.Core;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Log
{
    public interface IQQLogger : ILogger, IQQService
    {
        IQQContext Context { get; set; }
    }
}
