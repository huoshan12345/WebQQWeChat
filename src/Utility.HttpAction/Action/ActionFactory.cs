using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utility.Extensions;

namespace Utility.HttpAction.Action
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
                var paras = ctor.GetParameters(); // paras.Length must be larger than args.Length
                if (paras.Length != args.Length)
                {
                    var argsNew = new object[paras.Length];
                    args.CopyTo(argsNew, 0);
                    
                    for (var i = args.Length; i < paras.Length; i++)
                    {
                        argsNew[i] = paras[i].RawDefaultValue;
                    }
                    args = argsNew;
                }
            }
            if (ctor == null) throw new MissingMethodException();
            return (IAction)ctor.Invoke(args);
        }
    }
}
