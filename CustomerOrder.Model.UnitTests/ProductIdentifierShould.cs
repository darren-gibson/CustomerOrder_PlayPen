using System;
using NUnit.Framework;

namespace CustomerOrder.Model.UnitTests
{
    [TestFixture]
    public class ProductIdentifierShould
    {
        private const string ProductPrefix = "trn:tesco:product:uuid:";
        private const string GtinPrfix = "urn:epc:id:gtin:";

        [Test]
        public void StartWithTheCorrentPrefix()
        {
            ProductIdentifier identifier = string.Format("{0}{1}", ProductPrefix, "0957151D-6FA6-4DF7-866D-E4FF74B1EAE2");
            Assert.IsNotNull(identifier);
        }

        [Test]
        public void ThrowAnExceptionIfItDoesNotStartWithTheCorrentPrefix()
        {
            const string wrongPrefix = "trn:tesco:pr0duct:uuid:";
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = wrongPrefix + "0957151D-6FA6-4DF7-866D-E4FF74B1EAE2"; });
        }

        [Test]
        public void ThrowAnExceptionIfNotAGuid()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = ProductPrefix + "12345x678"; });
        }

        [Test]
        public void ThrowAnExceptionIfTheGuidStringIsNotValid()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = ProductPrefix + "0957151D-6FA6-4DF7-866D-E4F74B1EAE2"; });
        }

        [Test]
        public void ThrowAnExceptionIfGuidNotPresent()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = ProductPrefix; });
        }

        [Test]
        public void CreateAProductIdentifierFromAGuid()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier identifier = Guid.Parse(guidString);

            Assert.AreEqual(identifier.ToString(), ProductPrefix + guidString);
        }

        #region Equals and GetHashCode
        [Test]
        public void BeEqualIfTheyHaveTheSameIdentifierValue()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.Parse(guidString);

            Assert.AreEqual(productIdentifier1, productIdentifier2);
        }

        [Test]
        public void BeNotEqualIfTheyHaveDifferentIdentifierValues()
        {
            ProductIdentifier productIdentifier1 = Guid.NewGuid();
            ProductIdentifier productIdentifier2 = Guid.NewGuid();

            Assert.AreNotEqual(productIdentifier1, productIdentifier2);
            Assert.IsFalse(productIdentifier1.Equals(null));
        }

        [Test]
        public void ExplicitlyTestTheEqualsMethod()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.Parse(guidString);
            Assert.IsTrue(productIdentifier1.Equals(productIdentifier2));
            Assert.IsFalse(productIdentifier1.Equals(null));
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(productIdentifier1.Equals("hello"));
        }

        [Test]
        public void ExplicitlyTestTheEqualsOperator()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.Parse(guidString);
            Assert.IsTrue(productIdentifier1 == productIdentifier2);
            Assert.IsFalse(productIdentifier1 == Guid.NewGuid());
        }

        [Test]
        public void ExplicitlyTestTheNotEqualsOperator()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.Parse(guidString);
            Assert.IsFalse(productIdentifier1 != productIdentifier2);
            Assert.IsTrue(productIdentifier1 != Guid.NewGuid());
        }

        [Test]
        public void HashCodeForTwoEqualProductIdentifiersAreEqual()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.Parse(guidString);

            Assert.AreEqual(productIdentifier1.GetHashCode(), productIdentifier2.GetHashCode());
        }

        [Test]
        public void HashCodeForTwoDifferentProductIdentifiersAreNotEqual()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            ProductIdentifier productIdentifier1 = Guid.Parse(guidString);
            ProductIdentifier productIdentifier2 = Guid.NewGuid();

            Assert.AreNotEqual(productIdentifier1.GetHashCode(), productIdentifier2.GetHashCode());
        }

        #endregion

        #region GTIN based identifier

        [Test]
        public void SupportCreatingAProductIdentifierInAGtinFormat()
        {
            ProductIdentifier identifier = string.Format("{0}{1}", GtinPrfix, "123456789012");
            Assert.IsNotNull(identifier);
        }

        [Test]
        public void CreatingAGtinProductIdentifierFromALong()
        {
            ProductIdentifier identifier = 99999999999999;
            Assert.AreEqual(GtinPrfix + "99999999999999", identifier.ToString());
        }

        [Test]
        public void ThrowAnExceptionIfTheNumberIsMissing()
        {
            // ReSharper disable once UnusedVariable.Compiler
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = GtinPrfix; });
        }

        [Test]
        public void ThrowAnExceptionIfTheBarcodeContainsANonNumeric()
        {
            // ReSharper disable once UnusedVariable.Compiler
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = GtinPrfix + "123v123"; });
        }

        [Test]
        public void ThrowAnExceptionIfTheBarcodeIsTooLong()
        {
            // ReSharper disable once UnusedVariable.Compiler
            Assert.Throws<InvalidCastException>(() => { ProductIdentifier identifier = 1234567890121345; });
        }

        [Test]
        public void CorrectlyFormatAGtinProductIdentifierFromALong()
        {
            ProductIdentifier identifier = 1;
            Assert.AreEqual(GtinPrfix + "00000000000001", identifier.ToString());
        }

        [Test]
        public void CreateAGtinBasedProductIdentifierFromANumberOfString()
        {
            ProductIdentifier identifier = "11";
            Assert.AreEqual(GtinPrfix + "00000000000011", identifier.ToString());            
        }

        #endregion

    }
}

