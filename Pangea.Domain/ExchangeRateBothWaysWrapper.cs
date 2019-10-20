using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Wrap a given exchange rate provider to allow to use the same rate for conversion in both directions
    /// </summary>
    public class ExchangeRateBothWaysWrapper : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _rates;
        /// <summary>
        /// Wrap a given exchange rate provider to allow to use the same rate for conversion in both directions
        /// </summary>
        /// <param name="rates">The provider to wrap</param>
        public ExchangeRateBothWaysWrapper(IExchangeRateProvider rates)
        {
            if (rates == null) throw new ArgumentNullException(nameof(rates));
            _rates = rates;
        }

        /// <summary>
        /// Try to get the exchange rate SOURCE->TARGET
        /// </summary>
        /// <param name="sourceCurrency">The currency to convert from</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>Either the exchange rate, or an exception</returns>
        public ExchangeRate TryGet(Currency sourceCurrency, Currency targetCurrency)
        {
            return
                _rates.TryGet(sourceCurrency, targetCurrency) ??
                _rates.TryGet(targetCurrency, sourceCurrency);
        }
    }
}
