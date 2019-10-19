namespace Pangea.Domain
{

    /// <summary>
    /// Type of conversion used in <see cref="ExchangeRateCollection"/>
    /// </summary>
    public enum ExchangeRateConversionType
    {
        /// <summary>
        /// The rate both ways is the same. 
        /// </summary>
        SameRateBothWays,
        /// <summary>
        /// There is a different rate specified for the inverse conversion.
        /// E.g. USD->EUR is a different rate than EUR->USD
        /// </summary>
        InverseRateIsDifferent
    }
}
