using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Add a wrapper around an <see cref="IExchangeRateProvider"/> to cache the results
    /// </summary>
    public class CachedExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _provider;
        private readonly ConcurrentDictionary<Tuple<Currency, Currency>, ExchangeRate> _rates;

        /// <summary>
        /// Create a cached version of the given <paramref name="provider"/>
        /// </summary>
        /// <param name="provider">The inner provider</param>
        public CachedExchangeRateProvider(IExchangeRateProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _rates = new ConcurrentDictionary<Tuple<Currency, Currency>, ExchangeRate>();
        }

        /// <inheritdoc/>
        public ExchangeRate TryGet(Currency source, Currency target)
        {
            var key = Tuple.Create(source, target);
            return _rates.GetOrAdd(key, key => _provider.TryGet(key.Item1, key.Item2));
        }
    }
}
