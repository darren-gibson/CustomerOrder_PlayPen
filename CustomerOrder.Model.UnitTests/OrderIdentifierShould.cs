using System;
using NUnit.Framework;

namespace CustomerOrder.Model.UnitTests
{
    using NUnit.Core;

    [TestFixture]
    public class OrderIdentifierShould
    {
        private const string OrderPrefix = "trn:tesco:order:uuid:";

        [Test]
        public void StartWithTheCorrentPrefix()
        {
            OrderIdentifier identifier = string.Format("{0}{1}", OrderPrefix, "0957151D-6FA6-4DF7-866D-E4FF74B1EAE2");
            Assert.IsNotNull(identifier);
        }

        [Test]
        public void ThrowAnExceptionIfItDoesNotStartWithTheCorrentPrefix()
        {
            const string wrongPrefix = "trn:tesco:pr0duct:uuid:";
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { OrderIdentifier identifier = wrongPrefix + "0957151D-6FA6-4DF7-866D-E4FF74B1EAE2"; });
        }

        [Test]
        public void ThrowAnExceptionIfNotAGuid()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { OrderIdentifier identifier = OrderPrefix + "12345x678"; });
        }

        [Test]
        public void ThrowAnExceptionIfTheGuidStringIsNotValid()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { OrderIdentifier identifier = OrderPrefix + "0957151D-6FA6-4DF7-866D-E4F74B1EAE2"; });
        }

        [Test]
        public void ThrowAnExceptionIfGuidNotPresent()
        {
            // ReSharper disable once UnusedVariable.Compiler - Under Test
            Assert.Throws<InvalidCastException>(() => { OrderIdentifier identifier = OrderPrefix; });
        }

        [Test]
        public void CreateAOrderIdentifierFromAGuid()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier identifier = Guid.Parse(guidString);

            Assert.AreEqual(identifier.ToString(), OrderPrefix + guidString);
        }

        #region Equals and GetHashCode
        [Test]
        public void BeEqualIfTheyHaveTheSameIdentifierValue()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.Parse(guidString);

            Assert.AreEqual(orderIdentifier1, orderIdentifier2);
        }

        [Test]
        public void ExplicitlyTestTheEqualsMethod()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.Parse(guidString);
            Assert.IsTrue(orderIdentifier1.Equals(orderIdentifier2));
            Assert.IsFalse(orderIdentifier1.Equals(null));
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(orderIdentifier1.Equals("hello"));
        }

        [Test]
        public void ExplicitlyTestTheEqualsOperator()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.Parse(guidString);
            Assert.IsTrue(orderIdentifier1 == orderIdentifier2);
            Assert.IsFalse(orderIdentifier1 == Guid.NewGuid());
        }

        [Test]
        public void ExplicitlyTestTheNotEqualsOperator()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.Parse(guidString);
            Assert.IsFalse(orderIdentifier1 != orderIdentifier2);
            Assert.IsTrue(orderIdentifier1 != Guid.NewGuid());
        }

        [Test]
        public void BeNotEqualIfTheyHaveDifferentIdentifierValues()
        {
            OrderIdentifier orderIdentifier1 = Guid.NewGuid();
            OrderIdentifier orderIdentifier2 = Guid.NewGuid();

            Assert.AreNotEqual(orderIdentifier1, orderIdentifier2);
            Assert.IsFalse(orderIdentifier1.Equals(null));
        }

        [Test]
        public void HashCodeForTwoEqualOrderIdentifiersAreEqual()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.Parse(guidString);

            Assert.AreEqual(orderIdentifier1.GetHashCode(), orderIdentifier2.GetHashCode());
        }

        [Test]
        public void HashCodeForTwoDifferentOrderIdentifiersAreNotEqual()
        {
            const string guidString = "0957151d-6fa6-4df7-866d-e4ff74b1eae2";
            OrderIdentifier orderIdentifier1 = Guid.Parse(guidString);
            OrderIdentifier orderIdentifier2 = Guid.NewGuid();

            Assert.AreNotEqual(orderIdentifier1.GetHashCode(), orderIdentifier2.GetHashCode());
        }

        #endregion
    }
}

