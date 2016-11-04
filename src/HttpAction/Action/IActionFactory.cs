using System;

namespace HttpAction.Action
{
    public interface IActionFactory
    {
        IAction CreateAction(Type actionType, params object[] parameters);
    }
}
