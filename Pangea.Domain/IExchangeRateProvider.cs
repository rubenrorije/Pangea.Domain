namespace Pangea.Domain
{
    /// <summary>
    /// Most basic interface to create a way to get exchange rates from a source
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Try to find an exchange rate to convert <paramref name="source"/> to <paramref name="target"/>
        /// </summary>
        /// <param name="source">The currency to convert from</param>
        /// <param name="target">The currency to convert to</param>
        /// <returns>The exchange rate when found, <c>null</c> otherwise</returns>
        ExchangeRate TryGet(Currency source, Currency target);
    }
}