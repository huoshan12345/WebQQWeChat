using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Action;
using WebQQ.Im.Core;

namespace WebQQ.Im.Service.Interface
{
    public interface IQQActionFactory : IActionFactory
    {
        /// <summary>
        /// 对象上下文
        /// </summary>
        IQQContext Context { get; }
    }
}
