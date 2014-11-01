namespace CustomerOrder.Model
{
    using System;

    public class CurrencyDoesNotMatchOrderException : Exception
    {
        public CurrencyDoesNotMatchOrderException(Currency orderCurrency, Currency requestedCurrency) : 
            base(string.Format("Currency {0} does not match order currency of {1}", requestedCurrency, orderCurrency))
        {
            
        }
    }
}
