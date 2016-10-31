using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.HttpAction.Action
{
    public interface IActionFactory
    {
        IAction CreateAction(Type actionType, params object[] parameters);
    }
}
