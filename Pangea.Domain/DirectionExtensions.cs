using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;

namespace Pangea.Domain
{
    /// <summary>
    /// Convenience methods for <see cref="Direction"/>
    /// </summary>
    public static class DirectionExtensions
    {
        private static readonly Dictionary<string, string> _intercardinalDirectionsWithArrows =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "N", "↑"},
                { "NE","↗" },
                { "E","→" },
                { "SE", "↘"},
                { "S", "↓"},
                { "SW", "↙"},
                { "W","←" },
                { "NW","↖" }
            };

        /// <summary>
        /// Get the directions for the given precision
        /// </summary>
        /// <param name="direction">the type of directions</param>
        /// <returns>A list of all directions of the precision</returns>
        public static IReadOnlyCollection<string> Get(this Direction direction)
        {
            switch (direction)
            {
                case Direction.InterCardinal:
                    return new[] { "NE", "SE", "SW", "NW" };
                case Direction.SecondaryInterCardinal:
                    return new[] { "NNE", "ENE", "ESE", "SSE", "SSW", "WSW", "WNW", "NNW" };
                case Direction.Cardinal:
                default:
                    return new[] { "N", "E", "S", "W" };
            }
        }

        /// <summary>
        /// Return all directions for the given precision, including the 'higher' precisions
        /// </summary>
        /// <param name="direction">The precision of the direction</param>
        /// <returns>A list of all directions for the given precision and 'higher' precisions</returns>
        public static IReadOnlyCollection<string> GetAll(this Direction direction)
        {
            switch (direction)
            {
                case Direction.InterCardinal:
                    return _intercardinalDirectionsWithArrows.Keys;
                case Direction.SecondaryInterCardinal:
                    return new[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
                case Direction.Cardinal:
                default:
                    return direction.Get();
            }
        }

        /// <summary>
        /// Convert a string representation of a direction into an arrow
        /// </summary>
        /// <param name="direction">the cardinal or inter cardinal direction</param>
        /// <returns>a string representing the direction</returns>
        /// <exception cref="ArgumentException">An unknown direction</exception>
        public static string ToArrow(string direction)
        {
            if (string.IsNullOrEmpty(direction)) throw new ArgumentNullException(nameof(direction));

            if (!_intercardinalDirectionsWithArrows.TryGetValue(direction, out string result))
            {
                throw new ArgumentException(Resources.Direction_InvalidDirectionCannotBeConvertedToArrow, nameof(direction));
            }
            return result;
        }
    }
}