namespace Pangea.Domain
{
    /// <summary>
    /// The types of directions on a windrose
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// The Cardinal directions on a windrose (N, E, S, W)
        /// </summary>
        Cardinal = 4,
        /// <summary>
        /// The intercardinal directions on a windrose (NE, SE, SW, NW)
        /// </summary>
        InterCardinal = 8,
        /// <summary>
        /// The secondary intercardinal directions on a windrose. E.g. NNE, WSW, etc.
        /// </summary>
        SecondaryInterCardinal = 16
    }
}