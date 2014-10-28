namespace CustomerOrder.Model.UnitTests.Events
{
    using System;
    using Model.Events;
    using NUnit.Framework;

    [TestFixture]
    public class ProductAddedEventShould
    {
        private Quantity _expectedQuantity;
        private ProductIdentifier _expectedIdentifier;
        private ProductAddedEvent _productAdded;

        [SetUp]
        public void SetUp()
        {
            _expectedIdentifier = Guid.NewGuid();
            _expectedQuantity = new Quantity(2, UnitOfMeasure.ML);
            _productAdded = new ProductAddedEvent(_expectedIdentifier, _expectedQuantity);
        }

        [Test]
        public void ContainTheProductThatHasBeenAdded()
        {
            Assert.AreEqual(_expectedIdentifier, _productAdded.ProductIdentifier);
            Assert.AreEqual(_expectedQuantity, _productAdded.Quantity);
        }

        [Test]
        public void HaveAUniqueIdentifier()
        {
            var secondProductAdded = new ProductAddedEvent(_expectedIdentifier, _expectedQuantity);
            Assert.AreNotEqual(_productAdded.EventId, secondProductAdded.EventId);
        }
    }
}

