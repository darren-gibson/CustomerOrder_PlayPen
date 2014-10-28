namespace CustomerOrder.Model
{
    using System;
    using System.Windows.Markup;
    using System.Xml.Schema;

    [Serializable]
    public struct Quantity : IFormattable
    {
        private readonly decimal _amount;
        private readonly UnitOfMeasure _unitOfMeasure;

        public Quantity(decimal amount, UnitOfMeasure unitOfMeasure)
        {
            _amount = amount;
            _unitOfMeasure = unitOfMeasure;
        }

        public static Quantity Default { get { return new Quantity(1, UnitOfMeasure.Each); } }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Quantity && Equals((Quantity)obj);
        }

        private bool Equals(Quantity other)
        {
            return _amount == other._amount && _unitOfMeasure == other._unitOfMeasure;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_amount.GetHashCode() * 397) ^ (int)_unitOfMeasure;
            }
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                format = "a u";
            return format.Replace("a", _amount.ToString(formatProvider)).Replace("u", _unitOfMeasure.ToString());
        }

        public static implicit operator Quantity(int amount)
        {
            return new Quantity(amount, UnitOfMeasure.Each);
        }

        public static bool operator ==(Quantity left, Quantity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Quantity left, Quantity right)
        {
            return !(left == right);
        }

        public static decimal operator /(Quantity left, Quantity right)
        {
            EnsureCompatibleUnitOfMeasures(left, right);
            return left._amount / right._amount;
        }

        private static void EnsureCompatibleUnitOfMeasures(Quantity left, Quantity right)
        {
            if (left._unitOfMeasure != right._unitOfMeasure)
                throw new IncompatibleUnitOfMeasureException(left._unitOfMeasure, right._unitOfMeasure);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}
