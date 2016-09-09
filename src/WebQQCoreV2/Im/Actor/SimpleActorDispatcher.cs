using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    /// <summary>
    /// <para>单线程的内部分发器，可以同时使用多个QQ实例里</para>
    /// </summary>
    public class QQActorDispatcher : SimpleActorDispatcher, IQQActorDispatcher
    {
        public void Init(IQQContext context)
        {
            BeginExcute();
        }

        public void Destroy()
        {
            Dispose();
        }
    }
}
