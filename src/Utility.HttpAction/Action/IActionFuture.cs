using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.HttpAction.Action
{
    public interface IActionFuture : IActor
    {
        /// <summary>
        /// 放入一个action到执行队列末尾
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IActionFuture PushAction(IAction action);
    }
}
