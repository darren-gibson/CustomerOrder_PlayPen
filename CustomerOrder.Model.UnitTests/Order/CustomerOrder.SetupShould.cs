namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using Model.Order;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public abstract class CustomerOrderSetupShould
    {
        protected ICustomerOrder OrderUnderTest;
        protected CustomerOrderFactory Factory;
        protected Mock<IPrice> PriceMock;
        protected Mock<IPricedOrder> PricedOrderMock;

        [SetUp]
        public virtual void SetUp()
        {
            const Currency orderCurrency = Currency.NZD;
            PriceMock = new Mock<IPrice>();
            Factory = new CustomerOrderFactory(PriceMock.Object);
            PricedOrderMock = new Mock<IPricedOrder>();
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(orderCurrency, 0));
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            PriceMock.Setup(p => p.Price(It.Is<ICustomerOrder>(o => o.Id.Equals(orderIdentifier)))).Returns(PricedOrderMock.Object);
            OrderUnderTest = Factory.MakeCustomerOrder(orderIdentifier, orderCurrency);
        }
    }
}