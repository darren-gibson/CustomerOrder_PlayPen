namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using System.Linq;
    using Model.Events;
    using Model.Order;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderGetProductPriceShould : CustomerOrderSetupShould
    {
        #region GetProductPrice

        [Test]
        public void ReturnTheResultFromThePricedBasket()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(PriceMock.Object);
            var productAddedEvent = new ProductAddedEvent(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest = factory.MakeCustomerOrder(Guid.NewGuid(), Currency.CAD, new IEvent[] { productAddedEvent }, PricedOrderMock.Object);
            var expectedPrice = CreateProductPrice();
            PricedOrderMock.Setup(o => o.GetProductPrice(productAddedEvent)).Returns(expectedPrice);

            var actualPrice = OrderUnderTest.GetProductPrice(OrderUnderTest.Products.First());

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        private IProductPrice CreateProductPrice()
        {
            return new Mock<IProductPrice>().Object;
        }

        #endregion
    }
}