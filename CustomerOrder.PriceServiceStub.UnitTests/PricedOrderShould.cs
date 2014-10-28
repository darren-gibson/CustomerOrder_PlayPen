namespace CustomerOrder.PriceServiceStub.UnitTests
{
    using Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class PricedOrderShould
    {
        [Test]
        public void PriceAnOrderWithNoProductsInIt()
        {
            var priceGateway = new SimplePriceGateway();
            var customerOrderMock = new Mock<ICustomerOrder>();
            var pricedOrderUnderTest = priceGateway.Price(customerOrderMock.Object);

            Assert.AreEqual(new Money(Currency.GBP, 0), pricedOrderUnderTest.NetTotal);
        }
    }
}