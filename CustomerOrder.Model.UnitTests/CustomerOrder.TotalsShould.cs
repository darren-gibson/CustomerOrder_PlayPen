namespace CustomerOrder.Model.UnitTests
{
    using System;
    using Model.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderTotalsShould
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

        #region Totals
        [Test]
        public void ReturnTheNetTotalFromThePricedOrderInterface()
        {
            var expectedNetTotal = new Money(_orderUnderTest.Currency, 10.30m);
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(expectedNetTotal);

            var actualNet = _orderUnderTest.NetTotal;

            Assert.AreEqual(expectedNetTotal, actualNet);
        }

        [Test]
        public void ReturnThAmountPaidBySummingTheAmountsInTheTender()
        {
            const Currency currency = Currency.JPY;
            var p1 = new PaymentEvent(CreateCashTender(2.52m, currency));
            var p2 = new PaymentEvent(CreateCashTender(1.92m, currency));

            _orderUnderTest = _factory.MakeCustomerOrder(Guid.NewGuid(), currency, new IEvent[] {p1, p2}, _pricedOrderMock.Object);

            Assert.AreEqual(new Money(_orderUnderTest.Currency, 4.44m), _orderUnderTest.AmountPaid);
        }


        [Test]
        public void ReturnTheAmountDueToBeTheNetTotalMinusTheAmountPaid()
        {
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(_orderUnderTest.Currency, 10.30m));

            _orderUnderTest.PaymentAdd(CreateCashTender(2.52m, _orderUnderTest.Currency));
            _orderUnderTest.PaymentAdd(CreateCashTender(1.92m, _orderUnderTest.Currency));

            Assert.AreEqual(new Money(_orderUnderTest.Currency, 5.86m), _orderUnderTest.AmountDue);
        }

        private Tender CreateCashTender(decimal amount, Currency currency)
        {
            return new Tender(new Money(currency, amount), "Cash");
        }

        #endregion
    }
}