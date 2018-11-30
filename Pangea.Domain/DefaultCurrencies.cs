using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A default list of ten most used currencies to date
    /// </summary>
    public static class DefaultCurrencies
    {
        /// <summary>
        /// Australian dollar: AUD, 036, $
        /// </summary>
        public static readonly Currency AUD = new Currency("AUD", 036, "$");
        /// <summary>
        /// Canadian dollar: CAD, 124, $ 
        /// </summary>
        public static readonly Currency CAD = new Currency("CAD", 124, "$");
        /// <summary>
        /// Swiss franc: CHF, 756
        /// </summary>
        public static readonly Currency CHF = new Currency("CHF", 756);
        /// <summary>
        /// Renminbi (Chinese) yuan: CNY, 156, ¥
        /// </summary>
        public static readonly Currency CNY = new Currency("CNY", 156, "¥");
        /// <summary>
        /// Euro: EUR, 978, €
        /// </summary>
        public static readonly Currency EUR = new Currency("EUR", 978, "€");
        /// <summary>
        /// Pound Sterling: GBP, 826, £
        /// </summary>
        public static readonly Currency GBP = new Currency("GBP", 826, "£");
        /// <summary>
        /// Japanese Yen: JPY, 392, ¥
        /// </summary>
        public static readonly Currency JPY = new Currency("JPY", 392, "¥");
        /// <summary>
        /// New Zealand Dollar: NZD, 554, $
        /// </summary>
        public static readonly Currency NZD = new Currency("NZD", 554, "$");
        /// <summary>
        /// Swedish krona: SEK, 752, kr
        /// </summary>
        public static readonly Currency SEK = new Currency("SEK", 752, "kr");
        /// <summary>
        /// United States dollar: 840, $
        /// </summary>
        public static readonly Currency USD = new Currency("USD", 840, "$");

        /// <summary>
        /// Use these default currencies, the most traded currencies as the list of currencies to register.
        /// If no 
        /// </summary>
        public static void Register()
        {
            if (!Currencies.ProviderIsRegistered)
            {
                Currencies.SetEmptyProvider();
            }
            Currencies.Instance.AddRange(AUD, CAD, CHF, CNY, EUR, GBP, JPY, NZD, SEK, USD);
        }

    }
}
