﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pangea.Domain.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Pangea.Domain.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The creditcard is invalid because the checksum is incorrect.
        /// </summary>
        internal static string CreditCard_InvalidChecksum {
            get {
                return ResourceManager.GetString("CreditCard_InvalidChecksum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credit card must be less than 20 characters.
        /// </summary>
        internal static string CreditCard_LessThan20Characters {
            get {
                return ResourceManager.GetString("CreditCard_LessThan20Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credit card must be at least 8 characters.
        /// </summary>
        internal static string CreditCard_MoreThan8Characters {
            get {
                return ResourceManager.GetString("CreditCard_MoreThan8Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credit card can only contain digits or spaces.
        /// </summary>
        internal static string CreditCard_OnlyDigits {
            get {
                return ResourceManager.GetString("CreditCard_OnlyDigits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format string.
        /// </summary>
        internal static string Currency_InvalidFormat {
            get {
                return ResourceManager.GetString("Currency_InvalidFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provider function is not set. Use the SetProvider-function to register an instance. Probably you want to create a CurrencyCollection instance once for your application in the bootstrapping code and register it in your IoC-Container. After that you want to call the SetProvider with a function to retrieve the instance from your IoC-container. When you do not use an IoC-container in your application, you can create a new CurrencyCollection instance and use the following call: CurrencyCollection.SetProvider [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CurrencyCollection_NotInitialized {
            get {
                return ResourceManager.GetString("CurrencyCollection_NotInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create a date range with a smaller end date than the start date.
        /// </summary>
        internal static string DateRange_EndDateBeforeStartDate {
            get {
                return ResourceManager.GetString("DateRange_EndDateBeforeStartDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot combine the ranges because the ranges are not adjacent.
        /// </summary>
        internal static string DateRange_RangesNotAdjacent {
            get {
                return ResourceManager.GetString("DateRange_RangesNotAdjacent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot subtract the ranges, because the end dates do not match.
        /// </summary>
        internal static string DateRange_SubtractEndDatesDontMatch {
            get {
                return ResourceManager.GetString("DateRange_SubtractEndDatesDontMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No ranges are specified.
        /// </summary>
        internal static string DateRangeCollection_NoRanges {
            get {
                return ResourceManager.GetString("DateRangeCollection_NoRanges", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid direction to convert to arrow.
        /// </summary>
        internal static string Direction_InvalidDirectionCannotBeConvertedToArrow {
            get {
                return ResourceManager.GetString("Direction_InvalidDirectionCannotBeConvertedToArrow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Should have 2 characters, but has less characters.
        /// </summary>
        internal static string EmojiFlag_LessThan2Characters {
            get {
                return ResourceManager.GetString("EmojiFlag_LessThan2Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Should have 2 characters, but has more characters.
        /// </summary>
        internal static string EmojiFlag_MoreThan2Characters {
            get {
                return ResourceManager.GetString("EmojiFlag_MoreThan2Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It is not possible to create an exchange rate within the same currency.
        /// </summary>
        internal static string ExchangeRate_SameCurrency {
            get {
                return ResourceManager.GetString("ExchangeRate_SameCurrency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exchange rate is already added to the collection.
        /// </summary>
        internal static string ExchangeRateCollection_RateIsAlreadyAdded {
            get {
                return ResourceManager.GetString("ExchangeRateCollection_RateIsAlreadyAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find exchange rate for the given currencies.
        /// </summary>
        internal static string ExchangeRates_CannotFindExchangeRateForCurrencies {
            get {
                return ResourceManager.GetString("ExchangeRates_CannotFindExchangeRateForCurrencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The country code should be followed by 2 check digits (0-9).
        /// </summary>
        internal static string Iban_2CheckDigits {
            get {
                return ResourceManager.GetString("Iban_2CheckDigits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entered Iban is incorrect.
        /// </summary>
        internal static string Iban_EnteredIncorrect {
            get {
                return ResourceManager.GetString("Iban_EnteredIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the Iban must be at least 5 characters. 2 for the country code, 2 check digits and at least 1 for the country specific account number.
        /// </summary>
        internal static string Iban_LessThan5Characters {
            get {
                return ResourceManager.GetString("Iban_LessThan5Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the Iban can be at most 34 characters. 2 for the country code, 2 check digits and 30 for the country specific account number.
        /// </summary>
        internal static string Iban_MoreThan34Characters {
            get {
                return ResourceManager.GetString("Iban_MoreThan34Characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Iban can only contain letters (A-Z) and digits (0-9).
        /// </summary>
        internal static string Iban_OnlyLettersAndDigits {
            get {
                return ResourceManager.GetString("Iban_OnlyLettersAndDigits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Iban should start with the ISO country code (ALPHA-2).
        /// </summary>
        internal static string Iban_StartWithIso {
            get {
                return ResourceManager.GetString("Iban_StartWithIso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot add two money objects with different currencies.
        /// </summary>
        internal static string Money_CannotAddDifferentCurrencies {
            get {
                return ResourceManager.GetString("Money_CannotAddDifferentCurrencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot compare these money instances because they do not have the same currency.
        /// </summary>
        internal static string Money_CannotCompareDifferentCurrencies {
            get {
                return ResourceManager.GetString("Money_CannotCompareDifferentCurrencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot convert money into other currency using the exchange rate, because the currency of the money is not found in the exchange rate.
        /// </summary>
        internal static string Money_CannotConvertWithExchangeRate {
            get {
                return ResourceManager.GetString("Money_CannotConvertWithExchangeRate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only NumberFormatInfo and CultureInfo are supported formatproviders.
        /// </summary>
        internal static string Money_InvalidFormatProvider {
            get {
                return ResourceManager.GetString("Money_InvalidFormatProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid phone number.
        /// </summary>
        internal static string PhoneNumber_Invalid {
            get {
                return ResourceManager.GetString("PhoneNumber_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format string.
        /// </summary>
        internal static string PhoneNumberFormatterWrapper_InvalidFormat {
            get {
                return ResourceManager.GetString("PhoneNumberFormatterWrapper_InvalidFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path could not be added, because it is absolute.
        /// </summary>
        internal static string SmartFolder_CannotAddAbsolute {
            get {
                return ResourceManager.GetString("SmartFolder_CannotAddAbsolute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The folder is absolute and cannot be subtracted from another path.
        /// </summary>
        internal static string SmartFolder_SubtractAbsolute {
            get {
                return ResourceManager.GetString("SmartFolder_SubtractAbsolute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot subtract the path, because both paths are identical.
        /// </summary>
        internal static string SmartFolder_SubtractIdentical {
            get {
                return ResourceManager.GetString("SmartFolder_SubtractIdentical", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot subtract folder because they do not have a common ancestor directory.
        /// </summary>
        internal static string SmartFolder_SubtractNoCommonAncestor {
            get {
                return ResourceManager.GetString("SmartFolder_SubtractNoCommonAncestor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Variable cannot be empty.
        /// </summary>
        internal static string SmartFolder_VariableCannotBeEmpty {
            get {
                return ResourceManager.GetString("SmartFolder_VariableCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Volume cannot be empty.
        /// </summary>
        internal static string SmartFolder_VolumeCannotBeEmpty {
            get {
                return ResourceManager.GetString("SmartFolder_VolumeCannotBeEmpty", resourceCulture);
            }
        }
    }
}
