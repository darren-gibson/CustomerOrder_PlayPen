namespace CustomerOrder.Model.UnitTests
{
    using System;
    using System.Linq;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderShould
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
            _orderUnderTest = _factory.MakeCustomerOrder(orderIdentifier);
        }

        #region Construction
        [Test]
        public void ImplementICustomerOrder()
        {
            Assert.IsInstanceOf(typeof(ICustomerOrder), _orderUnderTest);
        }
        #endregion

        #region Id
        [Test]
        public void ReturnTheCustomerOrderIdThatWasPassedInOnTheConstructor()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(_priceMock.Object);
            OrderIdentifier expectedId = Guid.NewGuid();
            _orderUnderTest = factory.MakeCustomerOrder(expectedId);

            Assert.AreEqual(expectedId, _orderUnderTest.Id);
        }
        #endregion

        #region ProductAdd
        [Test]
        public void AddTheProductToTheOrderWhenTheProductAddIsCalled()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            var expectedQuantity = new Quantity(3, UnitOfMeasure.ML);
            _orderUnderTest.ProductAdd(expectedProductIdentifier, expectedQuantity);

            Assert.IsTrue(_orderUnderTest.Products.Any(p =>
                p.ProductIdentifier.Equals(expectedProductIdentifier) && p.Quantity == expectedQuantity),
                "Expect Products to contain product identifier");
        }

        [Test]
        public void RaiseAProductAddedEventWhenTheProductIsAdded()
        {
            IProduct productAddedInEvent = null;
            CustomerOrder customerOrderInEvent = null;
            _orderUnderTest.ProductAdded += (sender, args) =>
            {
                customerOrderInEvent = (CustomerOrder)sender;
                productAddedInEvent = (args.ProductAdded);
            };

            _orderUnderTest.ProductAdd(Guid.NewGuid(), new Quantity(1, UnitOfMeasure.Each));

            Assert.AreSame(_orderUnderTest, customerOrderInEvent);
            Assert.AreEqual(_orderUnderTest.Products.First(), productAddedInEvent);
        }

        [Test]
        public void RaiseAnOrderPricedEventWhenTheProductIsAdded()
        {
            CustomerOrder customerOrderInEvent = null;
            IPricedOrder pricedOrderInEvent = null;
            _orderUnderTest.OrderPriced += (sender, args) =>
            {
                customerOrderInEvent = (CustomerOrder)sender;
                pricedOrderInEvent = args.PricedOrder;
            };

            _orderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);

            Assert.AreSame(_orderUnderTest, customerOrderInEvent);
            Assert.AreEqual(_pricedOrderMock.Object, pricedOrderInEvent);
        }

        [Test]
        public void PriceTheProductAddedByCallingThePriceInterface()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            var expectedPrice = CreateProductPrice();
            _pricedOrderMock.Setup(o => o.GetProductPrice(It.Is<IProduct>(p => p.ProductIdentifier.Equals(expectedProductIdentifier)))).Returns(expectedPrice);

            _orderUnderTest.ProductAdded += (sender, args) => Assert.AreEqual(expectedPrice, args.Price);

            _orderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
        }

        [Test]
        public void HavePlacedTheProductInTheListOfProductsBeforeCallingThePriceOperation()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();

            _priceMock.Setup(price => price.Price(_orderUnderTest)).Returns(_pricedOrderMock.Object)
                .Callback(() =>
                        Assert.IsTrue(_orderUnderTest.Products.Any(p => p.ProductIdentifier == expectedProductIdentifier),
                        "The product needs to be added before Price is called"));

            _orderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
        }

        [Test]
        public void ReturnAProductAddedType()
        {
            ProductIdentifier expectedIdentifier = Guid.NewGuid();
            var expectedQuantity = new Quantity(3.3m, UnitOfMeasure.ML);
            var expectedPrice = CreateProductPrice(3.3m, 10);
            _pricedOrderMock.Setup(o => o.GetProductPrice(It.Is<IProduct>(p => p.ProductIdentifier.Equals(expectedIdentifier)))).Returns(expectedPrice);

            var productAdded = _orderUnderTest.ProductAdd(expectedIdentifier, expectedQuantity);

            Assert.AreEqual(expectedIdentifier, productAdded.ProductIdentifier);
            Assert.AreEqual(expectedQuantity, productAdded.Quantity);
            Assert.AreEqual(expectedPrice.UnitPrice, productAdded.UnitPrice);
            Assert.AreEqual(expectedPrice.NetPrice, productAdded.NetPrice);
        }

        [Test]
        public void ThrowAnyExceptionRaisedByThePricingService()
        {
            _priceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
            Assert.Throws<InvalidCastException>(() => _orderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default));
        }

        [Test]
        public void NotAddTheProductToTheListIfPricingThrowsAnException()
        {
            ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
            _priceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
            try
            {
                _orderUnderTest.ProductAdd(expectedProductIdentifier, Quantity.Default);
            }
            catch (InvalidCastException) { }
            Assert.IsFalse(_orderUnderTest.Products.Any(p =>
            p.ProductIdentifier.Equals(expectedProductIdentifier)), "Expect Products NOT to contain product identifier");
        }

        #endregion


        #region ProductAdd
        [Test]
        public void AddThePaymentToTheOrderWhenThePaymentAddIsCalled()
        {
            var expectedTender = GetTender();
            _orderUnderTest.PaymentAdd(expectedTender);

            Assert.IsTrue(_orderUnderTest.Payments.Any(p => p.Amount.Equals(expectedTender)), "Expect Payments to contain the Tender");
        }

        private static Tender GetTender()
        {
            return new Tender(new Money(Currency.CHF, 102.3m), "Cash");
        }

        [Test]
        public void ReturnAPaymentAddedType()
        {
            var expectedTender = GetTender();
            var paymentAdded = _orderUnderTest.PaymentAdd(expectedTender);

            Assert.AreEqual(expectedTender, paymentAdded.Amount);
        }

        /*

                [Test]
                public void RaiseAProductAddedEventWhenTheProductIsAdded()
                {
                    IProduct productAddedInEvent = null;
                    CustomerOrder customerOrderInEvent = null;
                    _orderUnderTest.ProductAdded += (sender, args) =>
                    {
                        customerOrderInEvent = (CustomerOrder)sender;
                        productAddedInEvent = (args.ProductAdded);
                    };

                    _orderUnderTest.ProductAdd(Guid.NewGuid(), new Quantity(1, UnitOfMeasure.Each));

                    Assert.AreSame(_orderUnderTest, customerOrderInEvent);
                    Assert.AreEqual(_orderUnderTest.Products.First(), productAddedInEvent);
                }

                [Test]
                public void RaiseAnOrderPricedEventWhenTheProductIsAdded()
                {
                    CustomerOrder customerOrderInEvent = null;
                    IPricedOrder pricedOrderInEvent = null;
                    _orderUnderTest.OrderPriced += (sender, args) =>
                    {
                        customerOrderInEvent = (CustomerOrder)sender;
                        pricedOrderInEvent = args.PricedOrder;
                    };

                    _orderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);

                    Assert.AreSame(_orderUnderTest, customerOrderInEvent);
                    Assert.AreEqual(_pricedOrderMock.Object, pricedOrderInEvent);
                }

                [Test]
                public void PriceTheProductAddedByCallingThePriceInterface()
                {
                    ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
                    var expectedPrice = CreateProductPrice();
                    _pricedOrderMock.Setup(o => o.GetProductPrice(It.Is<IProduct>(p => p.ProductIdentifier.Equals(expectedProductIdentifier)))).Returns(expectedPrice);

                    _orderUnderTest.ProductAdded += (sender, args) => Assert.AreEqual(expectedPrice, args.Price);

                    _orderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
                }

                [Test]
                public void HavePlacedTheProductInTheListOfProductsBeforeCallingThePriceOperation()
                {
                    ProductIdentifier expectedProductIdentifier = Guid.NewGuid();

                    _priceMock.Setup(price => price.Price(_orderUnderTest)).Returns(_pricedOrderMock.Object)
                        .Callback(() =>
                                Assert.IsTrue(_orderUnderTest.Products.Any(p => p.ProductIdentifier == expectedProductIdentifier),
                                "The product needs to be added before Price is called"));

                    _orderUnderTest.ProductAdd(expectedProductIdentifier, new Quantity(1, UnitOfMeasure.Each));
                }

                [Test]
                public void ThrowAnyExceptionRaisedByThePricingService()
                {
                    _priceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
                    Assert.Throws<InvalidCastException>(() => _orderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default));
                }

                [Test]
                public void NotAddTheProductToTheListIfPricingThrowsAnException()
                {
                    ProductIdentifier expectedProductIdentifier = Guid.NewGuid();
                    _priceMock.Setup(p => p.Price(It.IsAny<ICustomerOrder>())).Throws<InvalidCastException>();
                    try
                    {
                        _orderUnderTest.ProductAdd(expectedProductIdentifier, Quantity.Default);
                    }
                    catch (InvalidCastException) { }
                    Assert.IsFalse(_orderUnderTest.Products.Any(p =>
                    p.ProductIdentifier.Equals(expectedProductIdentifier)), "Expect Products NOT to contain product identifier");
                }

        */
        #endregion


        #region GetProductPrice

        [Test]
        public void ReturnTheResultFromThePricedBasket()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(_priceMock.Object);
            var productAddedEvent = new ProductAddedEvent(Guid.NewGuid(), Quantity.Default);
            _orderUnderTest = factory.MakeCustomerOrder(Guid.NewGuid(), new IEvent[] { productAddedEvent }, _pricedOrderMock.Object);
            var expectedPrice = CreateProductPrice();
            _pricedOrderMock.Setup(o => o.GetProductPrice(productAddedEvent)).Returns(expectedPrice);

            var actualPrice = _orderUnderTest.GetProductPrice(_orderUnderTest.Products.First());

            Assert.AreEqual(expectedPrice, actualPrice);
        }

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


        #endregion

        #region Totals
        [Test]
        public void ReturnTheNetTotalFromThePricedOrderInterface()
        {
            var expectedNetTotal = new Money(Currency.CAD, 10.30m);
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(expectedNetTotal);

            var actualNet = _orderUnderTest.NetTotal;

            Assert.AreEqual(expectedNetTotal, actualNet);
        }
        #endregion
    }
}