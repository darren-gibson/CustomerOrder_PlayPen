namespace CustomerOrder.Model.UnitTests
{
    using System;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProductAddedEventArgsShould
    {
        [Test]
        public void ContainTheInformationPassedInTheConstructor()
        {
            var expectedProduct = new ProductAddedEvent(Guid.NewGuid(), Quantity.Default);
            var expectedProductPrice = new Mock<IProductPrice>().Object;
            var eventArgsUnderTest = new ProductAddedEventArgs(expectedProduct, expectedProductPrice);

            Assert.AreEqual(expectedProduct, eventArgsUnderTest.ProductAdded);
            Assert.AreEqual(expectedProductPrice, eventArgsUnderTest.Price);
            Assert.AreEqual(expectedProduct.EventId, eventArgsUnderTest.Id);
        }
    }
}