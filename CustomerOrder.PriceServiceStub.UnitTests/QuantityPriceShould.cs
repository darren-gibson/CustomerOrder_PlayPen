namespace CustomerOrder.PriceServiceStub.UnitTests
{
    using System.Threading;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class QuantityPriceShould
    {
        [Test]
        public void MoneyCreatedAtDifferentTimesSShouldCompareEqualUsingTheEqualsMethod()
        {
            var m1 = new Money(Currency.USD, 10);
            Thread.Sleep(20);
            var m2 = new Money(Currency.USD, 10);
            Assert.IsTrue(m1.Equals(m2));
        }

        [Test]
        public void MoneyCreatedAtDifferentTimesSShouldCompareEqualUsingTheEqualsOperator()
        {
            var m1 = new Money(Currency.USD, 10);
            Thread.Sleep(20);
            var m2 = new Money(Currency.USD, 10);
            Assert.IsTrue(m1 == m2);
        }

        [Test]
        public void CompareAsEqualTwoQuantityPricesWithTheSameValue()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNotEqualTwoQuantityPricesWithDifferentQuantities()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.ML));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));

            Assert.IsFalse(q1.Equals(q2));
            Assert.IsFalse(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNotEqualQuantityPricesWhenTheMoneyAreNotTheSame()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 20), new Quantity(1, UnitOfMeasure.Each));

            Assert.IsFalse(q1.Equals(q2));
            Assert.IsFalse(q2.Equals(q1));
        }

        [Test]
        public void CompareAsNullWhenTheOtherObjectIsNull()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            Assert.IsFalse(q1.Equals(null));
        }

        [Test]
        public void CompareAsFalseIfTheOtherObjectIsNotAQuantityPrice()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(q1.Equals(string.Empty));
        }

        [Test]
        public void DifferentQuantityPricesWithDifferentMoneyHaveDifferentHashCodes()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 20), new Quantity(1, UnitOfMeasure.Each));

            Assert.AreNotEqual(q1.GetHashCode(), q2.GetHashCode());
        }

        [Test]
        public void DifferentQuantityPricesWithDifferentQuantityHaveDifferentHashCodes()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(2, UnitOfMeasure.Each));

            Assert.AreNotEqual(q1.GetHashCode(), q2.GetHashCode());
        }

        [Test]
        public void CompareAsEqualTwoQuantityPricesWithTheSameValueWithEqualOperator()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));

            Assert.IsTrue(q1 == q2);
        }
        [Test]
        public void CompareAsNotEqualTwoQuantityPricesWithTheSameValueWithNotEqualOperator()
        {
            var q1 = new QuantityPrice(new Money(Currency.AUD, 10), new Quantity(1, UnitOfMeasure.Each));
            var q2 = new QuantityPrice(new Money(Currency.AUD, 20), new Quantity(1, UnitOfMeasure.Each));

            Assert.IsTrue(q1 != q2);
        }

        [Test]
        public void HaveAFormattedToString()
        {
            var q1 = new QuantityPrice(new Money(Currency.USD, 1), new Quantity(1, UnitOfMeasure.Each));
            Assert.AreEqual("1.00 USD for 1 Each", q1.ToString());
        }

        [Test]
        public void SupportACustomFormattedUnitOfMeasureString()
        {
            Assert.AreEqual("ML", string.Format("{0:u}", new QuantityPrice(new Money(Currency.USD, 1), new Quantity(1, UnitOfMeasure.ML))));
            Assert.AreEqual("Each", string.Format("{0:u}", new QuantityPrice(new Money(Currency.USD, 1), new Quantity(1, UnitOfMeasure.Each))));
        }

        [Test]
        public void SupportACustomFormattedValueString()
        {
            Assert.AreEqual("4", string.Format("{0:a}", new QuantityPrice(new Money(Currency.USD, 1), new Quantity(4, UnitOfMeasure.ML))));
            Assert.AreEqual("123.456", string.Format("{0:a}", new QuantityPrice(new Money(Currency.USD, 1), new Quantity(123.456m, UnitOfMeasure.ML))));
        }

        [Test]
        public void SupportAFormattedPriceStringWithAValueAndCurrency()
        {
            var q1 = new QuantityPrice(new Money(Currency.USD, 1.70m), new Quantity(4, UnitOfMeasure.ML));
            Assert.AreEqual("1.70", q1.ToString("v", null));
            Assert.AreEqual("1.70 USD", string.Format("{0:v c}", q1));
            Assert.AreEqual("USD", string.Format("{0:c}", q1));
        }

        [Test]
        public void SupportAFormattedPriceAndQuantity()
        {
            var q1 = new QuantityPrice(new Money(Currency.GBP, 2.70m), new Quantity(1.2m, UnitOfMeasure.Each));
            Assert.AreEqual("2.70 GBP, 1.2 Each", string.Format("{0:v c, a u}", q1));
        }

        [Test]
        public void MultiplyByAQuantityToReturnThePrice()
        {
            var quantityPrice = new QuantityPrice(new Money(Currency.GBP, 2.70m), new Quantity(1, UnitOfMeasure.Each));
            var quantity = new Quantity(5, UnitOfMeasure.Each);
            var expectedPrice = new Money(Currency.GBP, 13.50m);

            var actualPrice = quantityPrice * quantity;
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [Test]
        public void ThrowAnExceptionIfTheUnitsOfMeasureDoNotEqual()
        {
            var quantityPrice = new QuantityPrice(new Money(Currency.GBP, 2.70m), new Quantity(1, UnitOfMeasure.Each));
            var quantity = new Quantity(5, UnitOfMeasure.ML);

            // ReSharper disable once UnusedVariable
            Assert.Throws<IncompatibleUnitOfMeasureException>(() => { var notUsed = quantityPrice*quantity; });
        }

        [Test]
        public void ConsiderThePriceAsAPricePerUnitOfMeasureAndScaleTheReturnToTheRightLevel()
        {
            var quantityPrice = new QuantityPrice(new Money(Currency.GBP, 0.20m), new Quantity(5, UnitOfMeasure.ML)); // 20p for 5ml
            var quantity = new Quantity(12.5m, UnitOfMeasure.ML); // buy 12.5ml
            var expectedPrice = new Money(Currency.GBP, 0.50m); // 12.5 ml @ 20p for 5ml (4p per ml) = 4 * 12.5 = 50p

            var actualPrice = quantityPrice * quantity;
            Assert.AreEqual(expectedPrice, actualPrice);
        }
    }
}

