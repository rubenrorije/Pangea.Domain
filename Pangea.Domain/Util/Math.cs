using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Util
{
    /// <summary>
    /// Convenient methods to calculate distance for GPS locations
    /// </summary>
    internal static class Math
    {
        /// <summary>
        /// Convert an angle to a radian value.
        /// </summary>
        /// <param name="angle">The angle in degrees to convert</param>
        /// <returns>the radians</returns>
        public static double ToRadians(this double angle)
        {
            return System.Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Returns the sine squared of the specified angle.
        /// </summary>
        /// <param name="angle">An angle, measured in radians.</param>
        /// <returns>The sine of a. If a is equal to System.Double.NaN, 
        /// System.Double.NegativeInfinity, or System.Double.PositiveInfinity, 
        /// this method returns System.Double.NaN.
        /// </returns>
        public static double Sin2(double angle)
        {
            return System.Math.Sin(angle) * System.Math.Sin(angle);
        }
        
    }
}
