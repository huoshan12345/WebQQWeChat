using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.HttpAction.Action
{
    public class ActionFactory : IActionFactory
    {
        public virtual IAction CreateAction(Type actionType, params object[] args)
        {
            if (actionType == null) throw new ArgumentNullException(nameof(actionType));
            if (actionType.IsAssignableFrom(typeof(IAction))) throw new ArgumentException(nameof(actionType));
            
            var argsType = args.Select(a => a.GetType()).ToArray();
            var ctor = actionType.GetConstructor(argsType);
            if (ctor == null)
            {
                ctor = actionType.GetConstructors().FirstOrDefault(m => m.GetParameters().Where(p => !p.IsOptional).Select(p => p.ParameterType).SequenceEqual(argsType));
                if (ctor != null)
                {
                    var paras = ctor.GetParameters();
                    var argsNew = new object[paras.Length];
                    for(int i = 0, j = 0; i < paras.Length; i++)
                    {
                        if (paras[i].IsOptional)
                        {
                            argsNew[i] = paras[i].RawDefaultValue;
                        }
                        else
                        {
                            argsNew[i] = args[j++];
                        }
                    }
                }
            }
            if(ctor == null) throw new Exception();
            return (IAction)ctor.Invoke(args);
        }
    }
}
