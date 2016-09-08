using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util
{
    public class Disposable : IDisposable
    {
        public static IDisposable Empty { get; } = new Disposable();
        public void Dispose()
        {
        }
    }
}
