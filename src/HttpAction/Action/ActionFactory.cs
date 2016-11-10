using System;
using System.Linq;
using System.Reflection;
using FxUtility.Extensions;

namespace HttpAction.Action
{
    public class ActionFactory : IActionFactory
    {
        public virtual IAction CreateAction<T>(params object[] parameters) where T : IAction
        {
            return (IAction)typeof(T).CreateObject(parameters);
        }
    }
}
