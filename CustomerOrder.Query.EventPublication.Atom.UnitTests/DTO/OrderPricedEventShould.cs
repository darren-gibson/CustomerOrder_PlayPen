using NUnit.Framework;

namespace CustomerOrder.Query.EventPublication.Atom.UnitTests.DTO
{
    using System;
    using Model;
    using Moq;
    using OrderPricedEvent = Atom.DTO.OrderPricedEvent;

    [TestFixture]
    public class OrderPricedEventShould : ICustomerOrderBasedEventShould<OrderPricedEvent>
    {
        [SetUp]
        public void SetUp()
        {
            var theOriginatingCustomerOrder = new Mock<ICustomerOrder>();
            theOriginatingCustomerOrder.SetupGet(o => o.Id).Returns("trn:tesco:order:uuid:1b4b0932-5854-489b-a77c-0cebd15d554b");
            var pricedOrderMock = new Mock<IPricedOrder>();
            pricedOrderMock.SetupGet(po => po.NetTotal).Returns(new Money(Currency.USD, 12.13m));

            EventUnderTest = new OrderPricedEvent(Guid.NewGuid(), theOriginatingCustomerOrder.Object, pricedOrderMock.Object);
        }

        protected override string GetExpectedXml()
        {
            return
                @"<OrderPricedEvent xmlns=""http://api.tesco.com/order/20140914"">" +
                    @"<order>trn:tesco:order:uuid:1b4b0932-5854-489b-a77c-0cebd15d554b</order>" +
                    @"<netTotal currency=""USD"">12.13</netTotal>" +
                @"</OrderPricedEvent>";
        }
    }
}

