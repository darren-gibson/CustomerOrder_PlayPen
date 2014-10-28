namespace CustomerOrder.Model.UnitTests
{
    using NUnit.Framework;

    [TestFixture]
    public class MoneyShould
    {
        #region Value Equality tests

        [Test]
        public void CompareAsEqualIfTwoMoneysAreCreatedWithTheSameValue()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.GBP, 1.2m);

            Assert.AreEqual(m1, m2);
            Assert.IsTrue(m1.Equals(m2));
        }

        [Test]
        public void CompareAsEqualUsingTheEqualOperator()
        {
            var m1 = new Money(Currency.GBP, 7.1221m);
            var m2 = new Money(Currency.GBP, 7.1221m);
            Assert.IsTrue(m1 == m2);
        }

        [Test]
        public void CompareAsNotEqualIfTwoMoneysAreCreatedWithDifferentValues()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.GBP, 1.1m);

            Assert.AreNotEqual(m1, m2);
            Assert.IsFalse(m1.Equals(m2));
        }

        [Test]
        public void CompareAsNotEqualIfTwoMoneysAreCreatedWithDifferentValuesUsingTheEqualOperator()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.GBP, 1.1m);

            Assert.IsFalse(m1 == m2);
            Assert.IsTrue(m1 != m2);
        }

        [Test]
        public void MiscEqualityChecks()
        {
            var m1 = new Money(Currency.USD, 123);
            Assert.AreNotEqual(m1, "hello");
            Assert.AreNotEqual(m1, null);
        }

        #endregion

        #region Currency Code Tests

        [Test]
        public void CompareAsNotEqualIfTwoMoneysAreCreatedWithDifferentCurrencyCodes()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.USD, 1.2m);

            Assert.AreNotEqual(m1, m2);
            Assert.IsFalse(m1.Equals(m2));
        }

        [Test]
        public void CompareAsNotEqualIfTwoMoneysAreCreatedWithDifferentCurrencyCodesUsingTheEqualOperator()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.JPY, 1.2m);

            Assert.IsFalse(m1 == m2);
            Assert.IsTrue(m1 != m2);
        }

        [Test]
        public void TheCurrencyCodeIsSetUsingTheConstructor()
        {
            var m = new Money(Currency.JPY, 1.2m);
            Assert.AreEqual(Currency.JPY, m.Code);
        }

        #endregion

        #region Addition Tests tests
        [Test]
        public void BeAbleToAddTwoMoneyObjectsTogether()
        {
            var m1 = new Money(Currency.USD, 10);
            var m2 = new Money(Currency.USD, 12.5m);

            Assert.AreEqual(new Money(Currency.USD, 22.5m), m1 + m2);
        }

        [Test]
        public void ThrowAnExceptionIfTheCurrenciesAddedAreNotConvertable()
        {
            var m1 = new Money(Currency.USD, 10);
            var m2 = new Money(Currency.GBP, 12.5m);

            // ReSharper disable once UnusedVariable
            Assert.Throws<UnableToConvertCurrencyException>(() => { var notUsed = m1 + m2; });
        }
        #endregion

        #region Subtraction Tests tests
        [Test]
        public void BeAbleToSubtractTwoMoneyObjectsFromOneAnother()
        {
            var m1 = new Money(Currency.JPY, 10);
            var m2 = new Money(Currency.JPY, 12.5m);

            Assert.AreEqual(new Money(Currency.JPY, -2.5m), m1 - m2);
        }

        [Test]
        public void ThrowAnExceptionIfTheCurrenciesSubtractedAreNotConvertable()
        {
            var m1 = new Money(Currency.USD, 10);
            var m2 = new Money(Currency.GBP, 12.5m);

            // ReSharper disable once UnusedVariable
            Assert.Throws<UnableToConvertCurrencyException>(() => { var notUsed = m1 - m2; });
        }
        #endregion

        #region Multiplication Tests
        [Test]
        public void BeAbleToMultiplyMoney()
        {
            var m1 = new Money(Currency.JPY, 11.1m);

            Assert.AreEqual(new Money(Currency.JPY, 55.5m), m1 * 5m);
        }

        #endregion

        #region Division Tests
        [Test]
        public void BeAbleToDivideMoney()
        {
            var m1 = new Money(Currency.CHF, 10);

            Assert.AreEqual(new Money(Currency.CHF, 0.5m), m1 / 20m);
        }

        #endregion

        #region ToString()

        [Test]
        public void OutputAFormattedString()
        {
            const string expected = "13.20 USD";
            var m1 = new Money(Currency.USD, 13.2m);
            Assert.AreEqual(expected, string.Format("{0}", m1));
            Assert.AreEqual(expected, m1.ToString());
        }

        [Test]
        public void OutputTheCurrencySymbolIfTheCFormatIsSpecified()
        {
            const string expected = "USD";
            var m1 = new Money(Currency.USD, 13.2m);
            Assert.AreEqual(expected, string.Format("{0:C}", m1));
        }

        [Test]
        public void PassTheFormatSpecifierToTheDefaultFormatIfOtherFormatIsSpecified()
        {
            const string expected = "13.2000";
            var m1 = new Money(Currency.USD, 13.2m);
            Assert.AreEqual(expected, string.Format("{0:n4}", m1));
        }

        #endregion

        #region GetHashCode

        [Test]
        public void ReturnTheSameHashCodeForTwoCreatedWithTheSameValue()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.GBP, 1.2m);

            Assert.AreEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [Test]
        public void ReturnADifferentHashCodeForTwoCreatedWithDifferentValues()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.GBP, 2.2m);

            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [Test]
        public void ReturnADifferentHashCodeForTwoCreatedWithDifferentCurrencyCodes()
        {
            var m1 = new Money(Currency.GBP, 1.2m);
            var m2 = new Money(Currency.USD, 1.2m);

            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        #endregion
    }
}