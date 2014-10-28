namespace CustomerOrder.Model
{
    using System;
    using System.Text.RegularExpressions;
    
    public struct OrderIdentifier
    {
        private const string OrderIdentifierPrefix = "trn:tesco:order:uuid:";
        private static readonly Regex FormatRegex
            = new Regex("^" + OrderIdentifierPrefix + @"([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})", RegexOptions.Compiled);
        private readonly string _value;

        private OrderIdentifier(string value)
        {
            AssertValidOrderIdentifierFormat(value);
            _value = value;
        }

        public static implicit operator OrderIdentifier(string identifier)
        {
            return new OrderIdentifier(identifier);
        }

        public static implicit operator OrderIdentifier(Guid uuid)
        {
            // TODO: Support converting a sting that is just a GUID
            return new OrderIdentifier(string.Format("{0}{1}", OrderIdentifierPrefix, uuid));
        }

        private static void AssertValidOrderIdentifierFormat(string identifier)
        {
            if (!FormatRegex.IsMatch(identifier))
                throw new InvalidCastException(string.Format("{0} is an invalid order identifier",
                    identifier));
        }

        public override string ToString()
        {
            return _value;
        }

        #region Equals & HashCode
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OrderIdentifier && Equals((OrderIdentifier) obj);
        }

        private bool Equals(OrderIdentifier other)
        {
            return string.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(OrderIdentifier a, OrderIdentifier b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(OrderIdentifier a, OrderIdentifier b)
        {
            return !(a == b);
        }

        #endregion
    }
}
