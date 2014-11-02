namespace CustomerOrder.Model.UnitTests
{
    using System;
    using System.Linq;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderGetProductPriceShould
    {
        private ICustomerOrder _orderUnderTest;
        private CustomerOrderFactory _factory;
        private Mock<IPrice> _priceMock;
        private Mock<IPricedOrder> _pricedOrderMock;

        [SetUp]
        public void SetUp()
        {
            _priceMock = new Mock<IPrice>();
            _factory = new CustomerOrderFactory(_priceMock.Object);
            _pricedOrderMock = new Mock<IPricedOrder>();
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _priceMock.Setup(p => p.Price(It.Is<ICustomerOrder>(o => o.Id.Equals(orderIdentifier)))).Returns(_pricedOrderMock.Object);
            _orderUnderTest = _factory.MakeCustomerOrder(orderIdentifier, Currency.NZD);
        }

        #region GetProductPrice

        [Test]
        public void ReturnTheResultFromThePricedBasket()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(_priceMock.Object);
            var productAddedEvent = new ProductAddedEvent(Guid.NewGuid(), Quantity.Default);
            _orderUnderTest = factory.MakeCustomerOrder(Guid.NewGuid(), Currency.CAD, new IEvent[] { productAddedEvent }, _pricedOrderMock.Object);
            var expectedPrice = CreateProductPrice();
            _pricedOrderMock.Setup(o => o.GetProductPrice(productAddedEvent)).Returns(expectedPrice);

            var actualPrice = _orderUnderTest.GetProductPrice(_orderUnderTest.Products.First());

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        private IProductPrice CreateProductPrice()
        {
            return new Mock<IProductPrice>().Object;
        }

        #endregion
    }
}