namespace CustomerOrder.Model
{
    using System;

    [Serializable]
    public class UnableToConvertCurrencyException : Exception
    {
        public UnableToConvertCurrencyException(Currency fromCurrency, Currency toCurrency) : base(string.Format("Unable to convert from {0} to {1}", fromCurrency, toCurrency))
        {
            
        }
    }
}
