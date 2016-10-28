using Microsoft.Extensions.Logging;
using WebQQ.Im.Core;

namespace WebQQ.Im.Service.Log
{
    public interface IQQLogger : ILogger, IQQService
    {
        IQQContext Context { get; set; }
    }
}
