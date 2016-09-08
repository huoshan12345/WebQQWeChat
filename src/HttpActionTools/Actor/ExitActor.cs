using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpActionTools.Actor
{
    /// <summary>
    /// 一个伪Actor只是为了让ActorLoop停下来
    /// </summary>
    public class ExitActor : IActor
    {
        public void Execute() { }

        public Task ExecuteAsync() => Task.Run(() => Execute());
    }
}
