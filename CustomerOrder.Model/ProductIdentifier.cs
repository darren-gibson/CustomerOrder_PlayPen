namespace CustomerOrder.Model
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    
    public struct ProductIdentifier
    {
        private const string ProductIdentifierPrefix = "trn:tesco:product:uuid:";
        private const string GtinPrefix = "urn:epc:id:gtin:";
        private static readonly Regex FormatRegex
            = new Regex("^(" + ProductIdentifierPrefix + @"([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}))|(" + GtinPrefix + @"([0-9]{1,14}))$", RegexOptions.Compiled);
        private readonly string _value;

        private ProductIdentifier(string value)
        {
            value = ConvertNumberStringToGtinFormat(value);
            AssertValidProductIdentifierFormat(value);
            _value = value;
        }

        private static string ConvertNumberStringToGtinFormat(string barcode)
        {
            long result;
            return long.TryParse(barcode, out result) ? 
                string.Format("{0}{1:d14}", GtinPrefix, result) : barcode;
        }

        public static implicit operator ProductIdentifier(string identifier)
        {
            return new ProductIdentifier(identifier);
        }

        public static implicit operator ProductIdentifier(Guid uuid)
        {
            return new ProductIdentifier(string.Format("{0}{1}", ProductIdentifierPrefix, uuid));
        }

        public static implicit operator ProductIdentifier(long barcode)
        {
            return new ProductIdentifier(barcode.ToString(CultureInfo.InvariantCulture));
        }

        private static void AssertValidProductIdentifierFormat(string identifier)
        {
            if (!FormatRegex.IsMatch(identifier) )
                throw new InvalidCastException(string.Format("{0} is an invalid product identifier", identifier));
        }

        public override string ToString()
        {
            return _value;
        }

        #region Equals & HashCode
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ProductIdentifier && Equals((ProductIdentifier) obj);
        }

        private bool Equals(ProductIdentifier other)
        {
            return string.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(ProductIdentifier a, ProductIdentifier b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ProductIdentifier a, ProductIdentifier b)
        {
            return !(a == b);
        }

        #endregion
    }
}
