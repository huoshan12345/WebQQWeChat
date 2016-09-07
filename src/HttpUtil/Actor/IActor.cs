using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpActionTools.Actor
{
    public interface IActor
    {
        void Execute();

        Task ExecuteAsync();
    }
}
