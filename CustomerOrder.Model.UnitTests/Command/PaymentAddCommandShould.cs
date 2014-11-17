namespace CustomerOrder.Model.UnitTests.Command
{
    using System;
    using System.Threading.Tasks;
    using Model.Command;
    using Model.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture] 
    public class PaymentAddCommandShould
    {
        private Mock<ICustomerOrderRepository> _orderRepositoryMock;
        private Mock<ICustomerOrder> _orderMock;
        private OrderIdentifier _orderIdentifier;

        [SetUp]
        public void SetUp()
        {
            _orderIdentifier = Guid.NewGuid();
            _orderRepositoryMock = new Mock<ICustomerOrderRepository>();
            _orderMock = new Mock<ICustomerOrder>();
            _orderRepositoryMock.Setup(r => r.GetOrCreateOrderById(_orderIdentifier)).Returns(Task.Factory.StartNew(() => _orderMock.Object));
        }

        [Test]
        public void GetTheOrderFromTheRepositoryAndCallPaymentAddWithThePayment()
        {
            var expectedTender = GetTenderAmount(123m);
            var commandUnderTest = new PaymentAddCommand(_orderRepositoryMock.Object, _orderIdentifier, expectedTender);

            commandUnderTest.Execute();

            _orderMock.Verify(o => o.PaymentAdd(expectedTender), Times.Once);
        }

        private Tender GetTenderAmount(decimal amountInCash)
        {
            return new Tender(new Money(Currency.GBP, amountInCash), "Cash");
        }


        [Test]
        public void SetTheIdWhenPassedViaTheConstructor()
        {
            var expectedId = Guid.NewGuid();
            var commandUnderTest = new PaymentAddCommand(_orderRepositoryMock.Object, _orderIdentifier,
                GetTenderAmount(123m), expectedId);
            Assert.AreEqual(expectedId, commandUnderTest.Id);
        }

        [Test]
        public void CreateAnIdIfOneIsNotSupplied()
        {
            var commandUnderTest = new PaymentAddCommand(_orderRepositoryMock.Object, _orderIdentifier,
                GetTenderAmount(12m));
            Assert.AreNotEqual(Guid.Empty, commandUnderTest.Id);
        }

        [Test]
        public void ReturnsThePaymentAddedItemWhenSucceeds()
        {
            var expectedTender = GetTenderAmount(123m);
            var expectedResult = new PaymentAdded(GetTenderAmount(123m));

            _orderMock.Setup(o => o.PaymentAdd(expectedTender)).Returns(expectedResult);

            var commandUnderTest = new PaymentAddCommand(_orderRepositoryMock.Object, _orderIdentifier, expectedTender);

            var result = commandUnderTest.Execute();
            Assert.AreSame(expectedResult, result);
        }
    }
}