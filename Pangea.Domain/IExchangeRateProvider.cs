using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Interface to get the exchange rate from an external source
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Get the latest exchange rate to convert 'from' to 'to'
        /// </summary>
        ExchangeRateAt Latest(Currency fromCurrency, Currency toCurrency);

        /// <summary>
        /// Returns the exchange rate at a given date to convert 'from' to 'to'
        /// </summary>
        ExchangeRateAt Historical(Currency fromCurrency, Currency toCurrency, DateTime exchangeRateDate);
    }
}
