using FluentAssertions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Util
{
    public static class TypeSelectorExtensions
    {

        /// <summary>
        /// Determines whether the namespace of type is exactly <paramref name="namespace"/>.
        /// </summary>
        public static TypeSelector ThatArePublic(this TypeSelector types)
        {
            return new TypeSelector(types.Where(t => t.IsPublic).ToList());
        }

        /// <summary>
        /// Determines whether the namespace of type is exactly <paramref name="namespace"/>.
        /// </summary>
        public static TypeSelector ThatArePublicOrInternal(this TypeSelector types)
        {
            /*
             The C# keywords protected and internal have no meaning in IL and are not used in the Reflection APIs. 
             The corresponding terms in IL are Family and Assembly. To identify an internal method using Reflection,
             use the IsAssembly property. To identify a protected internal method, use the IsFamilyOrAssembly.*/
            return new TypeSelector(types.Where(t => t.IsNestedAssembly || t.IsPublic).ToList());
        }
    }
}
