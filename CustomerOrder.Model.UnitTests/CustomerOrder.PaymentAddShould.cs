namespace CustomerOrder.Model.UnitTests
{
    using System;
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderPaymentAddShould
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

        #region PaymentAdd
        [Test]
        public void AddThePaymentToTheOrderWhenThePaymentAddIsCalled()
        {
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(_orderUnderTest.Currency, 200m));
            var expectedTender = CreateCashTender(102.3m, _orderUnderTest.Currency);
            _orderUnderTest.PaymentAdd(expectedTender);

            Assert.IsTrue(_orderUnderTest.Payments.Any(p => p.Amount.Equals(expectedTender)), "Expect Payments to contain the Tender");
        }

        [Test]
        public void ReturnAPaymentAddedType()
        {
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(_orderUnderTest.Currency, 200m));
            var expectedTender = CreateCashTender(102.3m, _orderUnderTest.Currency);
            var paymentAdded = _orderUnderTest.PaymentAdd(expectedTender);

            Assert.AreEqual(expectedTender, paymentAdded.Amount);
        }

        [Test]
        public void OnlyAllowTendersInTheOrderThatAreOfTheSameCurrencyAsTheOrder()
        {
            var tenderThatUsesADifferentCurrencyThanTheOrder = CreateCashTender(10m, Currency.CHF);
            Assert.Throws<CurrencyDoesNotMatchOrderException>(
                () => _orderUnderTest.PaymentAdd(tenderThatUsesADifferentCurrencyThanTheOrder));
        }

        [Test]
        public void ThrowAnExceptionIfAnAttemptIsMadeToOverTender()
        {
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(_orderUnderTest.Currency, 0));
            var amountGreaterThanOrderAmountDue = CreateCashTender(10m, _orderUnderTest.Currency);
            Assert.Throws<PaymentExceededAmountDueException>(() => _orderUnderTest.PaymentAdd(amountGreaterThanOrderAmountDue));            
        }

        [Test]
        public void IncludeTheAmountDueInTheException()
        {
            var expectedAmountDue = new Money(_orderUnderTest.Currency, 2.34m);
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(expectedAmountDue);
            try
            {
                _orderUnderTest.PaymentAdd(CreateCashTender(100m, _orderUnderTest.Currency));
            }
            catch (PaymentExceededAmountDueException e)
            {
                Assert.AreEqual(expectedAmountDue, e.AmountDue);
            }            
        }

        [Test]
        public void AllowsAPaymentUptoTheFullAmount() 
        {
            _pricedOrderMock.SetupGet(o => o.NetTotal).Returns(new Money(_orderUnderTest.Currency, 102.25m));
            _orderUnderTest.PaymentAdd(CreateCashTender(50m, _orderUnderTest.Currency));
            _orderUnderTest.PaymentAdd(CreateCashTender(50m, _orderUnderTest.Currency));
            Assert.Throws<PaymentExceededAmountDueException>(() => _orderUnderTest.PaymentAdd(CreateCashTender(2.26m, _orderUnderTest.Currency)));
            _orderUnderTest.PaymentAdd(CreateCashTender(2.25m, _orderUnderTest.Currency));
        }

        #endregion


        private Tender CreateCashTender(decimal amount, Currency currency)
        {
            return new Tender(new Money(currency, amount), "Cash");
        }

    }
}