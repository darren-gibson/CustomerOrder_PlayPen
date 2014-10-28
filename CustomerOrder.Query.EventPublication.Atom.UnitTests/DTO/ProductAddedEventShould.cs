using NUnit.Framework;

namespace CustomerOrder.Query.EventPublication.Atom.UnitTests.DTO
{
    using System;
    using Model;
    using Moq;
    using ProductAddedEvent = Atom.DTO.ProductAddedEvent;

    [TestFixture]
    public class ProductAddedEventShould : ICustomerOrderBasedEventShould<ProductAddedEvent>
    {
        [SetUp]
        public void SetUp()
        {
            var expectedUnitPrice = new Money(Currency.GBP, 123.45m);
            var expectedNetPrice = expectedUnitPrice * 2;
            var theOriginatingProductAddedEvent = new Model.Events.ProductAddedEvent("trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b", new Quantity(2, UnitOfMeasure.Each));
            var theOriginatingCustomerOrder = new Mock<ICustomerOrder>();
            var productPrice = new Mock<IProductPrice>();
            productPrice.SetupGet(p => p.NetPrice).Returns(expectedNetPrice);
            productPrice.SetupGet(p => p.UnitPrice).Returns(expectedUnitPrice);

            theOriginatingCustomerOrder.Setup(co => co.GetProductPrice(theOriginatingProductAddedEvent)).Returns(productPrice.Object);
            theOriginatingCustomerOrder.SetupGet(o => o.Id).Returns("trn:tesco:order:uuid:1b4b0932-5854-489b-a77c-0cebd15d554b");

            EventUnderTest = new ProductAddedEvent(Guid.NewGuid(), theOriginatingCustomerOrder.Object, theOriginatingProductAddedEvent);
        }

        protected override string GetExpectedXml()
        {
            return
                @"<ProductAddedEvent xmlns=""http://api.tesco.com/order/20140914"">" +
                    @"<order>trn:tesco:order:uuid:1b4b0932-5854-489b-a77c-0cebd15d554b</order>" +
                    @"<product>trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b</product>" +
                    @"<quantity uom=""Each"">2</quantity>" +
                    @"<price>" +
                        @"<unit currency=""GBP"">123.45</unit>" +
                        @"<net currency=""GBP"">246.90</net>" +
                    @"</price>" +
                @"</ProductAddedEvent>";
        }
    }
}

