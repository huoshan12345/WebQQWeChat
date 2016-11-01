using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool ArgumentListMatches(this MethodInfo m, Type[] args)
        {
            // If there are less arguments, then it just doesn't matter.
            var pInfo = m.GetParameters();
            if (pInfo.Length < args.Length)
                return false;

            // Now, check compatibility of the first set of arguments.
            var commonArgs = args.Zip(pInfo, (margs, pinfo) => Tuple.Create(margs, pinfo.ParameterType));
            if (commonArgs.Any(t => !t.Item1.IsAssignableFrom(t.Item2)))
                return false;

            // And make sure the last set of arguments are actually default!
            return pInfo.Skip(args.Length).All(p => p.IsOptional);
        }
    }
}
