using System;

namespace WebQQ.Util
{
    public class Disposable : IDisposable
    {
        public static IDisposable Empty { get; } = new Disposable();
        public void Dispose()
        {
        }
    }
}
