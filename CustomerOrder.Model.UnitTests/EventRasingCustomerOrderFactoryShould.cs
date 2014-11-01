namespace CustomerOrder.Model.UnitTests
{
    using System;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EventRasingCustomerOrderFactoryShould
    {
        private OrderIdentifier _expectedIdentifier;
        private ICustomerOrder _expectedOrder;
        private Mock<ICustomerOrderFactory> _mockFactory;
        private EventRasingCustomerOrderFactory _factoryUnderTest;
        private bool _customerOrderMadeWasCalled;
        private Currency _expectedCurrency;

        [SetUp]
        public void SetUp()
        {
            _expectedIdentifier = Guid.NewGuid();
            _expectedCurrency = Currency.INR;
            _expectedOrder = new Mock<ICustomerOrder>().Object;
            _mockFactory = new Mock<ICustomerOrderFactory>();
            _factoryUnderTest = new EventRasingCustomerOrderFactory(_mockFactory.Object);
            _customerOrderMadeWasCalled = false;
        }

        #region New Order
        [Test]
        public void CallsTheInnerFactoryAndReturnsTheOrderFromThatWhenANewOrderIsMade()
        {
            _mockFactory.Setup(f => f.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency)).Returns(_expectedOrder);
            var actualOrder = _factoryUnderTest.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency);
            Assert.AreEqual(_expectedOrder, actualOrder);
        }

        [Test]
        public void RaisesAnEventContainingTheOrderWhenANewOneIsMade()
        {
            _mockFactory.Setup(f => f.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency)).Returns(_expectedOrder);

            _factoryUnderTest.CustomerOrderMade += _factoryUnderTest_CustomerOrderMade;
            _factoryUnderTest.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency);
            Assert.IsTrue(_customerOrderMadeWasCalled);
        }
        #endregion

        #region Existing Order
        [Test]
        public void CallsTheInnerFactoryAndReturnsTheOrderFromThatWhenAnExistingOrderIsMade()
        {
            var expectedEvents = new IEvent[0];
            var expectedPricedOrder = new Mock<IPricedOrder>().Object;
            _mockFactory.Setup(f => f.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency, expectedEvents, expectedPricedOrder)).Returns(_expectedOrder);
            var actualOrder = _factoryUnderTest.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency, expectedEvents, expectedPricedOrder);
            Assert.AreEqual(_expectedOrder, actualOrder);
        }

        [Test]
        public void RaisesAnEventContainingTheOrderWhenAnExistingOneIsMade()
        {
            var expectedEvents = new IEvent[0];
            var expectedPricedOrder = new Mock<IPricedOrder>().Object;
            _mockFactory.Setup(f => f.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency, expectedEvents, expectedPricedOrder)).Returns(_expectedOrder);

            _factoryUnderTest.CustomerOrderMade += _factoryUnderTest_CustomerOrderMade;
            _factoryUnderTest.MakeCustomerOrder(_expectedIdentifier, _expectedCurrency, expectedEvents, expectedPricedOrder);
            Assert.IsTrue(_customerOrderMadeWasCalled);
        }
        #endregion

        void _factoryUnderTest_CustomerOrderMade(object sender, CustomerOrderMadeEventArgs e)
        {
            Assert.AreEqual(_expectedOrder, e.CustomerOrder);
            _customerOrderMadeWasCalled = true;
        }
    }
}