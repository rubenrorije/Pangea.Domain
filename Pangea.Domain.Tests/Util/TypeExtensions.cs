using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Util
{
    public static class TypeExtensions
    {
        public static bool ImplementsExplicitly<TInterface>(this Type type)
        {
            var map = type.GetInterfaceMap(typeof(TInterface));
            foreach (var method in map.TargetMethods)
            {
                if (!method.IsPrivate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
