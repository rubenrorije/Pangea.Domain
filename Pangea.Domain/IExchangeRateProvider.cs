using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Way to convert money from one currency to another using some internal exchange rates
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Try to get the exchange rate SOURCE->TARGET
        /// </summary>
        /// <param name="sourceCurrency">The currency to convert from</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>Either the exchange rate, or null when not found</returns>
        ExchangeRate TryGet(Currency sourceCurrency, Currency targetCurrency);
    }
}
