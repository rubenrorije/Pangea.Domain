using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Fluent
{
    /// <summary>
    /// Include this namespace to have a convenient way to create percentages.
    /// Whether you as a developer likes to have these extension methods is up to you by 
    /// (not) adding this namespace.
    /// </summary>
    public static class PercentageExtensions
    {
        /// <summary>
        /// Create a percentage using a fluent interface. Is equal to calling
        /// new Percentage(value)
        /// </summary>
        public static Percentage Percent(this int value)
        {
            return new Percentage(value);
        }
        /// <summary>
        /// Create a percentage using a fluent interface. Is equal to calling
        /// new Percentage(value)
        /// </summary>
        public static Percentage Percent(this double value)
        {
            return new Percentage(value);
        }
        /// <summary>
        /// Create a percentage using a fluent interface. Is equal to calling
        /// new Percentage(value)
        /// </summary>
        public static Percentage Percent(this decimal value)
        {
            return new Percentage(value);
        }

    }
}
