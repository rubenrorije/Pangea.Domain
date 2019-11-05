using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Represents an angle in degrees
    /// </summary>
    public struct Angle
        : IEquatable<Angle>
    {
        /// <summary>
        /// The degrees of the Angle
        /// </summary>
        public int Degrees { get; }

        /// <summary>
        /// Create a new Angle based on the number of degrees (0-360)
        /// </summary>
        /// <param name="degrees">The degrees (0-360)</param>
        public Angle(int degrees)
        {
            Degrees = degrees;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals((Angle)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Degrees.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(Angle other)
        {
            return Degrees == other.Degrees;
        }

        /// <inheritdoc/>
        public static bool operator ==(Angle left, Angle right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Angle left, Angle right)=> !(left == right);
    }
}
