using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    /// <summary>
    /// 一个伪Actor只是为了让ActorLoop停下来
    /// </summary>
    public class ExitActor : IQQActor
    {
        public void Execute()
        {
            //do nothing
        }

        public QQActorType Type => QQActorType.SimpleActor;

        public Task ExecuteAsync() => Task.Run(() => Execute());
    }
}
