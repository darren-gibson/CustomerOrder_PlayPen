namespace CustomerOrder.Model.UnitTests
{
    using System;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public abstract class CustomerOrderSetupShould
    {
        protected ICustomerOrder _orderUnderTest;
        protected CustomerOrderFactory _factory;
        protected Mock<IPrice> _priceMock;
        protected Mock<IPricedOrder> _pricedOrderMock;

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
    }
}