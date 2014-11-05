namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderPaymentAddShould : CustomerOrderSetupShould
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            // Must Have a product before payment can be made
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
        }

        #region PaymentAdd
        [Test]
        public void AddThePaymentToTheOrderWhenThePaymentAddIsCalled()
        {
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(OrderUnderTest.Currency, 200m));
            var expectedTender = CreateCashTender(102.3m, OrderUnderTest.Currency);
            OrderUnderTest.PaymentAdd(expectedTender);

            Assert.IsTrue(OrderUnderTest.Payments.Any(p => p.Amount.Equals(expectedTender)), "Expect Payments to contain the Tender");
        }

        [Test]
        public void ReturnAPaymentAddedType()
        {
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(OrderUnderTest.Currency, 200m));
            var expectedTender = CreateCashTender(102.3m, OrderUnderTest.Currency);
            var paymentAdded = OrderUnderTest.PaymentAdd(expectedTender);

            Assert.AreEqual(expectedTender, paymentAdded.Amount);
        }

        [Test]
        public void OnlyAllowTendersInTheOrderThatAreOfTheSameCurrencyAsTheOrder()
        {
            var tenderThatUsesADifferentCurrencyThanTheOrder = CreateCashTender(10m, Currency.CHF);
            Assert.Throws<CurrencyDoesNotMatchOrderException>(
                () => OrderUnderTest.PaymentAdd(tenderThatUsesADifferentCurrencyThanTheOrder));
        }

        [Test]
        public void ThrowAnExceptionIfAnAttemptIsMadeToOverTender()
        {
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(OrderUnderTest.Currency, 0));
            var amountGreaterThanOrderAmountDue = CreateCashTender(10m, OrderUnderTest.Currency);
            Assert.Throws<PaymentExceededAmountDueException>(() => OrderUnderTest.PaymentAdd(amountGreaterThanOrderAmountDue));            
        }

        [Test]
        public void IncludeTheAmountDueInTheException()
        {
            var expectedAmountDue = new Money(OrderUnderTest.Currency, 2.34m);
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(expectedAmountDue);
            try
            {
                OrderUnderTest.PaymentAdd(CreateCashTender(100m, OrderUnderTest.Currency));
            }
            catch (PaymentExceededAmountDueException e)
            {
                Assert.AreEqual(expectedAmountDue, e.AmountDue);
            }            
        }

        [Test]
        public void AllowsAPaymentUptoTheFullAmount() 
        {
            PricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(OrderUnderTest.Currency, 102.25m));
            OrderUnderTest.PaymentAdd(CreateCashTender(50m, OrderUnderTest.Currency));
            OrderUnderTest.PaymentAdd(CreateCashTender(50m, OrderUnderTest.Currency));
            Assert.Throws<PaymentExceededAmountDueException>(() => OrderUnderTest.PaymentAdd(CreateCashTender(2.26m, OrderUnderTest.Currency)));
            OrderUnderTest.PaymentAdd(CreateCashTender(2.25m, OrderUnderTest.Currency));
        }

        #endregion

        private Tender CreateCashTender(decimal amount, Currency currency)
        {
            return new Tender(new Money(currency, amount), "Cash");
        }

    }
}