namespace CustomerOrder.PriceServiceStub.UnitTests
{
    using Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class PricedOrderShould
    {
        private IPricedOrder _pricedOrderUnderTest;
        private Mock<ICustomerOrder> _customerOrderMock;
        private SimplePriceGateway _priceGateway;

        [SetUp]
        public void SetUp()
        {
            _priceGateway = new SimplePriceGateway();
            _customerOrderMock = new Mock<ICustomerOrder>();
            _customerOrderMock.SetupGet(o => o.Currency).Returns(Currency.GBP);
        }

        [Test]
        public void PriceAnOrderWithNoProductsInIt()
        {
            _pricedOrderUnderTest = _priceGateway.Price(_customerOrderMock.Object);
            Assert.AreEqual(new Money(Currency.GBP, 0), _pricedOrderUnderTest.NetTotal);
        }

        [Test]
        public void BeInTheSameCurrencyAsTheOrder()
        {
            _customerOrderMock.SetupGet(o => o.Currency).Returns(Currency.INR);
            _pricedOrderUnderTest = _priceGateway.Price(_customerOrderMock.Object);

            Assert.AreEqual(new Money(Currency.INR, 0), _pricedOrderUnderTest.NetTotal);
        }
    }
}