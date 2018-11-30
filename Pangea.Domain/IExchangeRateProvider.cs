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
        ExchangeRateAt Latest(Currency from, Currency to);

        /// <summary>
        /// Returns the exchange rate at a given date to convert 'from' to 'to'
        /// </summary>
        ExchangeRateAt Historical(Currency from, Currency to, DateTime date);
    }
}
