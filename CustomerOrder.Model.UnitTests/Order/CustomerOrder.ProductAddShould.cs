namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Model.Order;

    [TestFixture]
    public class CustomerOrderProductAddShould : CustomerOrderSetupShould
    {
        #region ProductAdd
        [Test]
        public void AddTheProductToTheOrderWhenTheProductAddIsCalled()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            var expectedQuantity = new Quantity(3, UnitOfMeasure.ML);
            OrderUnderTest.ProductAdd(expectedProductIdentifier, expectedQuantity);

            Assert.IsTrue(OrderUnderTest.Products.Any(p =>
                p.ProductIdentifier.Equals(expectedProductIdentifier) && p.Quantity == expectedQuantity),
                "Expect Products to contain product identifier");
        }

        [Test]
        public void RaiseAProductAddedEventWhenTheProductIsAdded()
        {
            IProduct productAddedInEvent = null;
            CustomerOrder customerOrderInEvent = null;
            OrderUnderTest.ProductAdded += (sender, args) =>
            {
                customerOrderInEvent = (CustomerOrder)sender;
                productAddedInEvent = (args.ProductAdded);
            };

            OrderUnderTest.ProductAdd(Guid.NewGuid(), new Quantity(1, UnitOfMeasure.Each));

            Assert.AreSame(OrderUnderTest, customerOrderInEvent);
            Assert.AreEqual(OrderUnderTest.Products.First(), productAddedInEvent);
        }

        [Test]
        public void RaiseAnOrderPricedEventWhenTheProductIsAdded()
        {
            CustomerOrder customerOrderInEvent = null;
            IPricedOrder pricedOrderInEvent = null;
            OrderUnderTest.OrderPriced += (sender, args) =>
            {
                customerOrderInEvent = (CustomerOrder)sender;
                pricedOrderInEvent = args.PricedOrder;
            };

            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);

            Assert.AreSame(OrderUnderTest, customerOrderInEvent);
            Assert.AreEqual(PricedOrderMock.Object, pricedOrderInEvent);
        }

        [Test]
        public void PriceTheProductAddedByCallingThePriceInterface()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            var expectedPrice = CreateProductPrice();
            PricedOrderMock.Setup(o => o.GetProductPrice(It.Is<IProduct>(p => p.ProductIdentifier.Equals(expectedProductIdentifier)))).Returns(expectedPrice);

            OrderUnderTest.ProductAdded += (sender, args) => Assert.AreEqual(expectedPrice, args.Price);

            OrderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
        }

        [Test]
        public void HavePlacedTheProductInTheListOfProductsBeforeCallingThePriceOperation()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();

            PriceMock.Setup(price => price.Price(OrderUnderTest)).Returns(PricedOrderMock.Object)
                .Callback(() =>
                        Assert.IsTrue(OrderUnderTest.Products.Any(p => p.ProductIdentifier == expectedProductIdentifier),
                        "The product needs to be added before Price is called"));

            OrderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
        }

        [Test]
        public void ReturnAProductAddedType()
        {
            ProductIdentifier expectedIdentifier = Guid.NewGuid();
            var expectedQuantity = new Quantity(3.3m, UnitOfMeasure.ML);
            var expectedPrice = CreateProductPrice(3.3m, 10);
            PricedOrderMock.Setup(o => o.GetProductPrice(It.Is<IProduct>(p => p.ProductIdentifier.Equals(expectedIdentifier)))).Returns(expectedPrice);

            var productAdded = OrderUnderTest.ProductAdd(expectedIdentifier, expectedQuantity);

            Assert.AreEqual(expectedIdentifier, productAdded.ProductIdentifier);
            Assert.AreEqual(expectedQuantity, productAdded.Quantity);
            Assert.AreEqual(expectedPrice.UnitPrice, productAdded.UnitPrice);
            Assert.AreEqual(expectedPrice.NetPrice, productAdded.NetPrice);
        }

        [Test]
        public void ThrowAnyExceptionRaisedByThePricingService()
        {
            PriceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
            Assert.Throws<InvalidCastException>(() => OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default));
        }

        [Test]
        public void NotAddTheProductToTheListIfPricingThrowsAnException()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            PriceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
            try
            {
                OrderUnderTest.ProductAdd(expectedProductIdentifier, Quantity.Default);
            }
            catch (InvalidCastException) { }
            Assert.IsFalse(OrderUnderTest.Products.Any(p =>
            p.ProductIdentifier.Equals(expectedProductIdentifier)), "Expect Products NOT to contain product identifier");
        }

        #endregion

        private IProductPrice CreateProductPrice()
        {
            return new Mock<IProductPrice>().Object;
        }

        private IProductPrice CreateProductPrice(decimal unit, decimal net)
        {
            var mockPrice = new Mock<IProductPrice>();
            mockPrice.SetupGet(p => p.NetPrice).Returns(new Money(Currency.AUD, net));
            mockPrice.SetupGet(p => p.UnitPrice).Returns(new Money(Currency.AUD, unit));
            return mockPrice.Object;
        }
    }
}