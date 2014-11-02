﻿namespace CustomerOrder.Model.UnitTests
{
    using System;
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
            _orderUnderTest = _factory.MakeCustomerOrder(orderIdentifier, Currency.NZD);
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
            _orderUnderTest = factory.MakeCustomerOrder(expectedId, Currency.INR);

            Assert.AreEqual(expectedId, _orderUnderTest.Id);
        }

        [Test]
        public void ReturnsTheCurrencyThatWasPassedInTheConstructor()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(_priceMock.Object);
            _orderUnderTest = factory.MakeCustomerOrder(Guid.NewGuid(), Currency.INR);

            Assert.AreEqual(Currency.INR, _orderUnderTest.Currency);
        }
        #endregion
    }
}