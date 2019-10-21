using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;

namespace Pangea.Domain
{
    /// <summary>
    /// Convenience methods for exchange rates
    /// </summary>
    public static class ExchangeRatesExtensions
    {
        /// <summary>
        /// Convert the amount into a different currency
        /// </summary>
        /// <param name="exchangeRates">The exchange rates</param>
        /// <param name="amount">The amount to convert</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>The converted currency</returns>
        public static  Money? TryConvert(this IExchangeRateProvider exchangeRates, Money amount, Currency targetCurrency)
        {
            if (exchangeRates == null) throw new ArgumentNullException(nameof(exchangeRates));
            var rate = exchangeRates.TryGet(amount.Currency, targetCurrency);
            if (rate == null) return null;
            return amount * rate;
        }


        /// <summary>
        /// Convert the amount into a different currency
        /// </summary>
        /// <param name="exchangeRates">The exchange rates</param>
        /// <param name="amount">The amount to convert</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>The converted currency</returns>
        ///<exception cref="KeyNotFoundException" />
        public static Money Convert(this IExchangeRateProvider exchangeRates, Money amount, Currency targetCurrency)
        {
            if (exchangeRates == null) throw new ArgumentNullException(nameof(exchangeRates));
            return
                exchangeRates.TryConvert(amount, targetCurrency) ??
                throw new KeyNotFoundException(Resources.ExchangeRates_CannotFindExchangeRateForCurrencies);

        }

        /// <summary>
        /// Try to get the exchange rate SOURCE->TARGET
        /// </summary>
        /// <param name="exchangeRates">The exchange rates</param>
        /// <param name="sourceCurrency">The currency to convert from</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>Either the exchange rate, or an exception</returns>
        /// <exception cref="KeyNotFoundException"/>
        public static ExchangeRate Get(this IExchangeRateProvider exchangeRates, Currency sourceCurrency, Currency targetCurrency)
        {
            if (exchangeRates == null) throw new ArgumentNullException(nameof(exchangeRates));
            return
                exchangeRates.TryGet(sourceCurrency, targetCurrency) ??
                throw new KeyNotFoundException(Resources.ExchangeRates_CannotFindExchangeRateForCurrencies);

        }
    }
}
