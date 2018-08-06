using System;
using System.Collections.Generic;
using System.Text;

namespace WebIm.Im.Core
{
    public interface IImModule : IDisposable
    {
        void Init();
    }

    public interface IImModule<out TContext> : IImModule
    {
        TContext Context { get; }
    }
}
