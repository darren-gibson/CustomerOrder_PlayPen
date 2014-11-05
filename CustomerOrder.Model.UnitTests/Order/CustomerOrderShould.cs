namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using Model.Order;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderShould : CustomerOrderSetupShould
    {
        #region Construction
        [Test]
        public void ImplementICustomerOrder()
        {
            Assert.IsInstanceOf(typeof(ICustomerOrder), OrderUnderTest);
        }
        #endregion

        #region Id
        [Test]
        public void ReturnTheCustomerOrderIdThatWasPassedInOnTheConstructor()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(PriceMock.Object);
            OrderIdentifier expectedId = Guid.NewGuid();
            OrderUnderTest = factory.MakeCustomerOrder(expectedId, Currency.INR);

            Assert.AreEqual(expectedId, OrderUnderTest.Id);
        }

        [Test]
        public void ReturnsTheCurrencyThatWasPassedInTheConstructor()
        {
            ICustomerOrderFactory factory = new CustomerOrderFactory(PriceMock.Object);
            OrderUnderTest = factory.MakeCustomerOrder(Guid.NewGuid(), Currency.INR);

            Assert.AreEqual(Currency.INR, OrderUnderTest.Currency);
        }
        #endregion
    }
}