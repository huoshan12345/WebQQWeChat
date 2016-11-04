using System;
using System.Linq;
using System.Reflection;
using FxUtility.Extensions;

namespace HttpAction.Action
{
    public class ActionFactory : IActionFactory
    {
        public virtual IAction CreateAction(Type actionType, params object[] args)
        {
            if (actionType == null) throw new ArgumentNullException(nameof(actionType));
            if (actionType.IsAssignableFrom(typeof(IAction))) throw new ArgumentException(nameof(actionType));

            var argsType = args.Select(a => a.GetType()).ToArray();
            var ctor = actionType.GetConstructors().FirstOrDefault(m => m.ArgumentListMatches(argsType));
            if (ctor != null)
            {
                var paras = ctor.GetParameters(); 
                if (paras.Length != args.Length)
                {   
                    // paras.Length must be larger than args.Length
                    var argsNew = new object[paras.Length];
                    args.CopyTo(argsNew, 0);
                    
                    for (var i = args.Length; i < paras.Length; i++)
                    {
                        argsNew[i] = paras[i].RawDefaultValue;
                    }
                    args = argsNew;
                }
            }
            else throw new MissingMethodException();
            return (IAction)ctor.Invoke(args);
        }
    }
}
