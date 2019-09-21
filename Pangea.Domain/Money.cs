using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Representation of an amount in a given currency
    /// </summary>
    [Serializable]
    public struct Money
        : IEquatable<Money>
        , IXmlSerializable
    {
        private readonly string _currency;
        private readonly decimal _amount;

        /// <summary>
        /// A given amount specified for a given currency
        /// </summary>
        public Money(Currency currency, decimal amount)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            _currency = currency.Code;
            _amount = amount;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals((Money)obj);
        }

        /// <summary>
        /// Is the amount and currency equal 
        /// </summary>
        /// <param name="other">The money to compare to</param>
        /// <returns>Whether the currency and amount are equal</returns>
        public bool Equals(Money other)
        {
            return
                _currency == other._currency &&
                _amount == other._amount;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {

            unchecked
            {
                var hash = 17;
                hash = hash * 23 + _currency.GetHashCode();
                hash = hash * 23 + _amount.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(Money left, Money right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Money left, Money right) => !(left == right);

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var amountValue = reader.MoveToAttribute("amount") ? reader.Value : null;
            var currencyValue = reader.MoveToAttribute("currency") ? reader.Value : null;


            if (string.IsNullOrEmpty(currencyValue))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                var amount = decimal.Parse(amountValue, CultureInfo.InvariantCulture);
                var currency = Currencies.Find(currencyValue);
                Unsafe.AsRef(this) = new Money(currency, amount);
            }
            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString("amount", _amount.ToString("G", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("currency", _currency);
        }
    }
}
