namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    using System;
    using Model;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderBasedEventPublisherShould
    {
        private CustomerOrderDouble _customerOrderDouble;
        private Mock<IAtomEventRepository> _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            OrderIdentifier identifier = Guid.NewGuid();
            var customerOrderFactoryMock = new Mock<ICustomerOrderFactory>();
            var factory = new EventRasingCustomerOrderFactory(customerOrderFactoryMock.Object);
            _customerOrderDouble = new CustomerOrderDouble();
            customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(identifier)).Returns(_customerOrderDouble);
            _repositoryMock = new Mock<IAtomEventRepository>();
            // ReSharper disable once ObjectCreationAsStatement - required for the test
            new CustomerOrderBasedEventPublisher(factory, _repositoryMock.Object);
            factory.MakeCustomerOrder(identifier);
        }

        #region ProductAdded Event
        [Test]
        public void SubscribeToTheProductAddedEventWhenANewOrderIsMade()
        {
            Assert.IsTrue(_customerOrderDouble.IsProductAddedSubscribed);
        }

        [Test]
        public void CallTheRepositoryToAddTheEventToTheCurrentFeedWhenAnProductAddedEventIsRaised()
        {
            ProductIdentifier expectedIdentifier = Guid.NewGuid();
            var productAddedEvent = new ProductAddedEvent(expectedIdentifier, Quantity.Default);
            _customerOrderDouble.RaiseProductAddedEvent(productAddedEvent);

            _repositoryMock.Verify(r => r.SaveEventToCurrentFeed(
                It.Is<CustomerOrderGeneratedEventSyndicationItem<Atom.DTO.ProductAddedEvent>>(i =>
                    i.Id.Equals(productAddedEvent.EventId.ToString()) &&
                    i.Content is CustomerOrderGeneratedEventSyndicationContent<Atom.DTO.ProductAddedEvent>)), Times.Once);
        }
        #endregion

        #region OrderPriced Event
        [Test]
        public void SubscribeToTheOrderPricedEventWhenANewOrderIsMade()
        {
            Assert.IsTrue(_customerOrderDouble.IsOrderPricedSubscribed);
        }

        [Test]
        public void CallTheRepositoryToAddTheEventToTheCurrentFeedWhenAnOrderPricedEventIsRaised()
        {
            _customerOrderDouble.RaiseOrderPricedEvent();

            _repositoryMock.Verify(r => r.SaveEventToCurrentFeed(
                It.Is<CustomerOrderGeneratedEventSyndicationItem<Atom.DTO.OrderPricedEvent>>(o =>
                    !string.IsNullOrEmpty(o.Id) &&
                    o.Content is CustomerOrderGeneratedEventSyndicationContent<Atom.DTO.OrderPricedEvent>)), Times.Once);
        }
        #endregion

        private sealed class CustomerOrderDouble : CustomerOrderBaseDouble
        {
            public void RaiseProductAddedEvent(ProductAddedEvent productAddedEvent)
            {
                OnProductAdded(new ProductAddedEventArgs(productAddedEvent, new Mock<IProductPrice>().Object));
            }

            public void RaiseOrderPricedEvent()
            {
                OnOrderPriced(new OrderPricedEventArgs(GetPricedOrder()));
            }

            private static IPricedOrder GetPricedOrder()
            {
                var pricedOrderMock = new Mock<IPricedOrder>();
                pricedOrderMock.SetReturnsDefault(new Money(Currency.USD, 10));
                return pricedOrderMock.Object;
            }

            public override IProductPrice GetProductPrice(IProduct product)
            {
                var mockPrice = new Mock<IProductPrice>();
                mockPrice.SetReturnsDefault(new Money(Currency.USD, 10));
                mockPrice.SetupAllProperties();

                return mockPrice.Object;
            }
        }
    }
}
