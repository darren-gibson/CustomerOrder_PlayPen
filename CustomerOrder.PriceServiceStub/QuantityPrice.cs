namespace CustomerOrder.PriceServiceStub
{
    using System;
    using Model;

    public struct QuantityPrice : IFormattable
    {
        private readonly Money _price;
        private readonly Quantity _quantity;

        public QuantityPrice(Money price, Quantity quantity)
        {
            _price = price;
            _quantity = quantity;
        }

        public Currency Currency { get { return _price.Code; } }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is QuantityPrice && Equals((QuantityPrice)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_price.GetHashCode() * 397) ^ _quantity.GetHashCode();
            }
        }

        private bool Equals(QuantityPrice other)
        {
            return _price.Equals(other._price) && _quantity.Equals(other._quantity);
        }

        public static bool operator ==(QuantityPrice left, QuantityPrice right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(QuantityPrice left, QuantityPrice right)
        {
            return !(left == right);
        }

        public static Money operator *(QuantityPrice quantityPrice, Quantity quantity)
        {
            var amount = quantity / quantityPrice._quantity;
            return quantityPrice._price * amount;
        }

        public override string ToString()
        {
            return string.Format("{0} for {1}", _price, _quantity);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format.Replace("v", _price.ToString("n", formatProvider));
            format = format.Replace("c", _price.Code.ToString());
            format = _quantity.ToString(format, formatProvider);
            return format;
        }
    }
}
