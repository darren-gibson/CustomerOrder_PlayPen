namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using Model.Events;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderTotalsShould : CustomerOrderSetupShould
    {
        #region Totals
        [Test]
        public void ReturnTheNetTotalFromThePricedOrderInterface()
        {
            var expectedNetTotal = new Money(OrderUnderTest.Currency, 10.30m);
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(expectedNetTotal);

            var actualNet = OrderUnderTest.NetTotal;

            Assert.AreEqual(expectedNetTotal, actualNet);
        }

        [Test]
        public void ReturnThAmountPaidBySummingTheAmountsInTheTender()
        {
            const Currency currency = Currency.JPY;
            var p1 = new PaymentEvent(CreateCashTender(2.52m, currency));
            var p2 = new PaymentEvent(CreateCashTender(1.92m, currency));

            OrderUnderTest = Factory.MakeCustomerOrder(Guid.NewGuid(), currency, new IEvent[] {p1, p2}, PricedOrderMock.Object);

            Assert.AreEqual(new Money(OrderUnderTest.Currency, 4.44m), OrderUnderTest.AmountPaid);
        }


        [Test]
        public void ReturnTheAmountDueToBeTheNetTotalMinusTheAmountPaid()
        {
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(OrderUnderTest.Currency, 10.30m));
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);

            OrderUnderTest.PaymentAdd(CreateCashTender(2.52m, OrderUnderTest.Currency));
            OrderUnderTest.PaymentAdd(CreateCashTender(1.92m, OrderUnderTest.Currency));

            Assert.AreEqual(new Money(OrderUnderTest.Currency, 5.86m), OrderUnderTest.AmountDue);
        }

        private Tender CreateCashTender(decimal amount, Currency currency)
        {
            return new Tender(new Money(currency, amount), "Cash");
        }

        #endregion
    }
}