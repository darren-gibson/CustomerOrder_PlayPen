namespace CustomerOrder.Model.UnitTests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderShould : CustomerOrderSetupShould
    {
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