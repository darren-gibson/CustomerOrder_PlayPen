namespace CustomerOrder.Model
{
    using System;

    // ReSharper disable once CSharpWarnings::CS0660
    public struct Money : IFormattable
    {
        private readonly Currency _currency;
        private readonly decimal _amount;

        public Money(Currency currency, decimal amount)
        {
            _currency = currency;
            _amount = amount;
        }

        public bool Equals(Money other)
        {
            return _currency == other._currency && _amount == other._amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Money && Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)_currency * 397) ^ _amount.GetHashCode();
            }
        }

        public static bool operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }
        public static Money operator +(Money left, Money right)
        {
            EnsureCurrencyIsConvertable(left.Code, right.Code);
            return new Money(left.Code, left._amount + right._amount);
        }
        public static Money operator -(Money left, Money right)
        {
            EnsureCurrencyIsConvertable(left.Code, right.Code);
            return new Money(left.Code, left._amount - right._amount);
        }
        public static Money operator *(Money left, decimal right)
        {
            return new Money(left.Code, left._amount * right);
        }
        public static Money operator /(Money left, decimal right)
        {
            return new Money(left.Code, left._amount / right);
        }

        private static void EnsureCurrencyIsConvertable(Currency fromCurrency, Currency toCurrency)
        {
            if (fromCurrency != toCurrency)
                throw new UnableToConvertCurrencyException(fromCurrency, toCurrency);
        }

        public Currency Code { get { return _currency; } }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return string.Format("{0:n} {1}", _amount, _currency);
            if (format.Equals("C", StringComparison.InvariantCultureIgnoreCase))
                return _currency.ToString();
            return _amount.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}
