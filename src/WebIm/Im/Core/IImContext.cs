using System;
using System.Collections.Generic;
using System.Text;
using FclEx.Http.Services;
using Microsoft.Extensions.Logging;

namespace WebIm.Im.Core
{
    public interface IImContext
    {
        T GetModule<T>() where T : IImModule;
        ILogger Logger { get; }
        IHttpService Http { get; }
    }
}
