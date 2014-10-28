
namespace CustomerOrder.Model.UnitTests
{
    using NUnit.Framework;

    [TestFixture]
    public class QuantityShould
    {
        [Test]
        public void CompareAsEqualTwoQuantitiesWithTheSameValue()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            var q2 = new Quantity(123, UnitOfMeasure.Each);

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNotEqualTwoQuantitiesWithDifferentUnitsOfMeasure()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            var q2 = new Quantity(123, UnitOfMeasure.ML);

            Assert.IsFalse(q1.Equals(q2));
            Assert.IsFalse(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNotEqualQuantitiesWhenTheValuesAreNotTheSame()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            var q2 = new Quantity(123.1m, UnitOfMeasure.Each);

            Assert.IsFalse(q1.Equals(q2));
            Assert.IsFalse(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNullWhenTheOtherObjectIsNull()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            Assert.IsFalse(q1.Equals(null));
        }

        [Test]
        public void CompareAsFalseIfTheOtherObjectIsNotAQuantity()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(q1.Equals(string.Empty));
        }

        [Test]
        public void DifferentQuantitiesHaveDifferentHashCodes()
        {
            var q1 = new Quantity(1, UnitOfMeasure.Each);
            var q2 = new Quantity(2, UnitOfMeasure.Each);

            Assert.AreNotEqual(q1.GetHashCode(), q2.GetHashCode());
        }

        [Test]
        public void DifferentUnitOfMeasuresHaveDifferentHashCodes()
        {
            var q1 = new Quantity(2, UnitOfMeasure.Each);
            var q2 = new Quantity(2, UnitOfMeasure.ML);

            Assert.AreNotEqual(q1.GetHashCode(), q2.GetHashCode());
        }

        [Test]
        public void CompareAsEqualTwoQuantitiesWithTheSameValueWithEqualOperator()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            var q2 = new Quantity(123, UnitOfMeasure.Each);

            Assert.IsTrue(q1 == q2);
        }

        [Test]
        public void CompareAsNotEqualTwoQuantitiesWithTheSameValueWithNotEqualOperator()
        {
            var q1 = new Quantity(123, UnitOfMeasure.Each);
            var q2 = new Quantity(50, UnitOfMeasure.ML);

            Assert.IsTrue(q1 != q2);
        }

        [Test]
        public void CreateAQuantityFromAnIntegerWithAUnitOfMeasureOfEach()
        {
            Quantity quantityCreatedWithAnInteger = 5;
            Assert.AreEqual(new Quantity(5, UnitOfMeasure.Each), quantityCreatedWithAnInteger);
        }

        [Test]
        public void OutputAFormatedString()
        {
            var quantity = new Quantity(5.4321m, UnitOfMeasure.ML);
            Assert.AreEqual("5.4321 ML", quantity.ToString());
        }

        [Test]
        public void SupportACustomFormattedUnitOfMeasureString()
        {
            Assert.AreEqual("ML", string.Format("{0:u}", new Quantity(1m, UnitOfMeasure.ML)));
            Assert.AreEqual("Each", string.Format("{0:u}", new Quantity(1m, UnitOfMeasure.Each)));
        }

        [Test]
        public void SupportACustomFormattedValueString()
        {
            Assert.AreEqual("4", string.Format("{0:a}", new Quantity(4, UnitOfMeasure.ML)));
            Assert.AreEqual("123.456", string.Format("{0:a}", new Quantity(123.456m, UnitOfMeasure.ML)));
        }

        [Test]
        public void SupportAFormattedStringWithAValueAndUnitOfMeasure()
        {
            Assert.AreEqual("12.3 ML", string.Format("{0:a u}", new Quantity(12.3m, UnitOfMeasure.ML)));
            Assert.AreEqual("1-Each", string.Format("{0:a-u}", new Quantity(1m, UnitOfMeasure.Each)));
        }

        [Test]
        public void CanDivideOneUnitOfMeasureByTheOther()
        {
            var tenMill = new Quantity(10, UnitOfMeasure.ML);
            var twoHundredMill = new Quantity(200, UnitOfMeasure.ML);

            Assert.AreEqual(0.05m, tenMill / twoHundredMill);
        }

        [Test]
        public void CannotDivdieQuantitiesIfTheUnitOfMeasuresAreNotTheSame()
        {
            var tenMill = new Quantity(10, UnitOfMeasure.ML);
            var oneEach = new Quantity(1, UnitOfMeasure.Each);

            // ReSharper disable once UnusedVariable
            Assert.Throws<IncompatibleUnitOfMeasureException>(() => { var notUsed = tenMill / oneEach; });
        }

        [Test]
        public void ThereIsADefaultQuantityOfOneEach()
        {
            Assert.AreEqual(new Quantity(1, UnitOfMeasure.Each), Quantity.Default);
        }
    }
}

