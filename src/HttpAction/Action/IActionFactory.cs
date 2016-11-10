using System;

namespace HttpAction.Action
{
    public interface IActionFactory
    {
        IAction CreateAction<T>(params object[] parameters) where T : IAction;
    }
}
