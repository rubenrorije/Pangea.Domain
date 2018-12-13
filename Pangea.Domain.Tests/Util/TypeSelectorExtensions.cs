using FluentAssertions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static TypeSelector ThatAreStructs(this TypeSelector types)
        {
            return new TypeSelector(types.Where(t => t.IsValueType && !t.IsEnum).ToList());
        }

        public static TypeSelector ThatHaveAConstructorWithArguments<T1>(this TypeSelector types)
        {
            return types.ThatHaveAConstructorWithArguments(typeof(T1));
        }

        public static TypeSelector ThatHaveAConstructorWithArguments(this TypeSelector types, params Type[] constructorArgs)
        {
            return
                new TypeSelector(
                    types
                    .Where(t => t.GetConstructors().Any(c => HasArguments(c, constructorArgs))).ToList());
        }

        private static bool HasArguments(this ConstructorInfo constructor, params Type[] arguments)
        {
            var parameters = constructor.GetParameters().Select(p => p.ParameterType).ToList();
            return parameters.SequenceEqual(arguments);
        }
    }
}
