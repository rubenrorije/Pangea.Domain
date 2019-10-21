namespace Pangea.Domain
{
    public partial struct CreditCard
    {
        /// <summary>
        /// Represent how the issuer is formatted. Since 2017 the issuer can be 8 (Long) characters,
        /// before only 6 (Short) characters were used to identify the issuer
        /// </summary>
        public enum CreditCardIssuerFormat
        {
            /// <summary>
            /// The issuer identifier is 8 characters long
            /// </summary>
            LongIdentifier = 8,
            /// <summary>
            /// The issuer identifier is 6 characters long
            /// </summary>
            ShortIdentifier = 6
        }

    }
}
