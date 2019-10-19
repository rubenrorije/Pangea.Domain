using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Pangea.Domain.DefaultCurrencies;
namespace Pangea.Domain.Fluent
{
    /// <summary>
    /// Fluent extension methods for Money
    /// </summary>
    public static class MoneyExtensions
    {
        /// <summary>
        /// Create a money instance that represents the amount in Australian Dollars (AUD).
        /// </summary>
        public static Money AustralianDollars(this decimal amount) => new Money(AUD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Canadian Dollars (CAD).
        /// </summary>
        public static Money CanadianDollars(this decimal amount) => new Money(CAD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Swiss Francs (CHF).
        /// </summary>
        public static Money SwissFrancs(this decimal amount) => new Money(CHF, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Chinese Yuan (CNY).
        /// </summary>
        public static Money ChineseYuan(this decimal amount) => new Money(CNY, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Euro (EUR).
        /// </summary>
        public static Money Euros(this decimal amount) => new Money(EUR, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Brittish Pounds (GBP).
        /// </summary>
        public static Money BritishPounds(this decimal amount) => new Money(GBP, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Japanese Yuan (JPY).
        /// </summary>
        public static Money JapaneseYen(this decimal amount) => new Money(JPY, amount);

        /// <summary>
        /// Create a money instance that represents the amount in New Zealand Dollars (NZD).
        /// </summary>
        public static Money NewZealandDollars(this decimal amount) => new Money(NZD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Swedish Krona (SEK).
        /// </summary>
        public static Money SwedishKrona(this decimal amount) => new Money(SEK, amount);

        /// <summary>
        /// Create a money instance that represents the amount in US Dollars (USD).
        /// </summary>
        public static Money Dollars(this decimal amount) => new Money(USD, amount);
        /// <summary>
        /// Create a money instance that represents the amount in Australian Dollars (AUD).
        /// </summary>
        public static Money AustralianDollars(this int amount) => new Money(AUD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Canadian Dollars (CAD).
        /// </summary>
        public static Money CanadianDollars(this int amount) => new Money(CAD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Swiss Francs (CHF).
        /// </summary>
        public static Money SwissFrancs(this int amount) => new Money(CHF, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Chinese Yuan (CNY).
        /// </summary>
        public static Money ChineseYuan(this int amount) => new Money(CNY, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Euro (EUR).
        /// </summary>
        public static Money Euros(this int amount) => new Money(EUR, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Brittish Pounds (GBP).
        /// </summary>
        public static Money BritishPounds(this int amount) => new Money(GBP, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Japanese Yuan (JPY).
        /// </summary>
        public static Money JapaneseYen(this int amount) => new Money(JPY, amount);

        /// <summary>
        /// Create a money instance that represents the amount in New Zealand Dollars (NZD).
        /// </summary>
        public static Money NewZealandDollars(this int amount) => new Money(NZD, amount);

        /// <summary>
        /// Create a money instance that represents the amount in Swedish Krona (SEK).
        /// </summary>
        public static Money SwedishKrona(this int amount) => new Money(SEK, amount);

        /// <summary>
        /// Create a money instance that represents the amount in US Dollars (USD).
        /// </summary>
        public static Money Dollars(this int amount) => new Money(USD, amount);

        /// <summary>
        /// Sum all items
        /// </summary>
        public static Money Sum(this IEnumerable<Money> items)
        {
            if (items == null) return default;
            if (!items.Any()) return default;

            var result = default(Money);
            foreach (var item in items)
            {
                result += item;
            }
            return result;
        }
        /// <summary>
        /// Average of all items
        /// </summary>
        public static Money Average(this IEnumerable<Money> items)
        {
            if (items == null) return default;
            if (!items.Any()) return default;

            var count = 0;
            var result = default(Money);
            foreach (var item in items)
            {
                result += item;
                count++;
            }
            return result / count;
        }
    }
}
