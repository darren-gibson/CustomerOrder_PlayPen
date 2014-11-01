namespace CustomerOrder.Model
{
    public struct Tender
    {
        private readonly string _tenderType;
        private readonly Money _amount;

        public Tender(Money amount, string tenderType)
        {
            _tenderType = tenderType;
            _amount = amount;
        }

        public bool Equals(Tender other)
        {
            return _tenderType == other._tenderType && _amount == other._amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Tender && Equals((Tender)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_tenderType.GetHashCode() * 397) ^ _amount.GetHashCode();
            }
        }

        public static bool operator ==(Tender left, Tender right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Tender left, Tender right)
        {
            return !(left == right);
        }
        public static Tender operator +(Tender left, Tender right)
        {
            EnsureTenderTypeIsCompatible(left.TenderType, right.TenderType);
            return new Tender(left._amount + right._amount, left.TenderType);
        }

        private static void EnsureTenderTypeIsCompatible(string fromTenderType, string toTenderType)
        {
            if (fromTenderType != toTenderType)
                throw new IncompatibleTenderTypeException(fromTenderType, toTenderType);
        }

        public string TenderType { get { return _tenderType; } }
        public Money Amount { get { return _amount; } }

        public override string ToString()
        {
            return string.Format("{0} in {1}", _amount, TenderType);
        }

    }
}
