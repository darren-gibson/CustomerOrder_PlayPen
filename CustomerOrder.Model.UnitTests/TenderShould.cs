namespace CustomerOrder.Model.UnitTests
{
    using NUnit.Framework;

    public class TenderShould
    {
        private const string Cash = "Cash";
        private const string Visa = "Visa";

        #region Value Equality tests

        [Test]
        public void CompareAsEqualIfTwoTendersAreCreatedWithTheSameValue()
        {
            var t1 = new Tender(GetMoney(123m), Cash);
            var t2 = new Tender(GetMoney(123m), Cash);

            Assert.AreEqual(t1, t2);
            Assert.IsTrue(t1.Equals(t2));
        }

        [Test]
        public void CompareAsEqualUsingTheEqualOperator()
        {
            var t1 = new Tender(GetMoney(7.1221m), Cash);
            var t2 = new Tender(GetMoney(7.1221m), Cash);
            Assert.IsTrue(t1 == t2);
        }

        [Test]
        public void CompareAsNotEqualIfTwoTendersAreCreatedWithDifferentValues()
        {
            var t1 = new Tender(GetMoney(1.2m), Cash);
            var t2 = new Tender(GetMoney(1.1m), Cash);

            Assert.AreNotEqual(t1, t2);
            Assert.IsFalse(t1.Equals(t2));
        }

        [Test]
        public void CompareAsNotEqualIfTwoTendersAreCreatedWithDifferentValuesUsingTheEqualOperator()
        {
            var t1 = new Tender(GetMoney(1.2m), Cash);
            var t2 = new Tender(GetMoney(1.1m), Cash);

            Assert.IsFalse(t1 == t2);
            Assert.IsTrue(t1 != t2);
        }

        [Test]
        public void MiscEqualityChecks()
        {
            var t1 = new Tender(GetMoney(123), Visa);
            Assert.AreNotEqual(t1, "hello");
            Assert.AreNotEqual(t1, null);
        }

        #endregion

        #region Currency Code Tests

        [Test]
        public void CompareAsNotEqualIfTwoTendersAreCreatedWithDifferentTenderTypes()
        {
            var t1 = new Tender(GetMoney(123m), Cash);
            var t2 = new Tender(GetMoney(123m), Visa);

            Assert.AreNotEqual(t1, t2);
            Assert.IsFalse(t1.Equals(t2));
        }

        [Test]
        public void CompareAsNotEqualIfTwoTendersAreCreatedWithDifferentTenderTypesUsingTheEqualOperator()
        {
            var t1 = new Tender(GetMoney(123m), Cash);
            var t2 = new Tender(GetMoney(123m), Visa);

            Assert.IsFalse(t1 == t2);
            Assert.IsTrue(t1 != t2);
        }

        [Test]
        public void TheTenderTypeIsSetUsingTheConstructor()
        {
            var t1 = new Tender(GetMoney(123m), Visa);
            Assert.AreEqual(Visa, t1.TenderType);
        }

        #endregion

        #region Amount Tests

        [Test]
        public void TheAmountIsSetUsingTheConstructor()
        {
            var expectedAmount = GetMoney(923m);
            var t1 = new Tender(expectedAmount, Visa);
            Assert.AreEqual(expectedAmount, t1.Amount);
        }

        #endregion

        #region Addition Tests tests
        [Test]
        public void BeAbleToAddTwoTenderObjectsTogether()
        {
            var t1 = new Tender(GetMoney(10m), Cash);
            var t2 = new Tender(GetMoney(12.5m), Cash);

            Assert.AreEqual(new Tender(GetMoney(22.5m), Cash), t1 + t2);
        }

        [Test]
        public void ThrowAnExceptionIfTheCurrenciesAddedAreNotConvertable()
        {
            var t1 = new Tender(GetMoney(123m), Cash);
            var t2 = new Tender(GetMoney(123m), Visa);

            // ReSharper disable once UnusedVariable
            Assert.Throws<IncompatibleTenderTypeException>(() => { var notUsed = t1 + t2; });
        }
        #endregion

        #region ToString()

        [Test]
        public void OutputAFormattedString()
        {
            const string expected = "13.20 USD in Cash";
            var t1 = new Tender(new Money(Currency.USD, 13.2m), Cash);
            Assert.AreEqual(expected, string.Format("{0}", t1));
            Assert.AreEqual(expected, t1.ToString());
        }

        #endregion

        #region GetHashCode

        [Test]
        public void ReturnTheSameHashCodeForTwoCreatedWithTheSameValue()
        {
            var t1 = new Tender(GetMoney(1.2m), Cash);
            var t2 = new Tender(GetMoney(1.2m), Cash);

            Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [Test]
        public void ReturnADifferentHashCodeForTwoCreatedWithDifferentValues()
        {
            var t1 = new Tender(GetMoney(1.2m), Visa);
            var t2 = new Tender(GetMoney(2.2m), Visa);

            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [Test]
        public void ReturnADifferentHashCodeForTwoCreatedWithDifferentCurrencyCodes()
        {
            var t1 = new Tender(GetMoney(1.2m), Visa);
            var t2 = new Tender(GetMoney(1.2m), Cash);

            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        #endregion

        private Money GetMoney(decimal amount)
        {
            return new Money(Currency.AUD, amount);
        }

    }
}