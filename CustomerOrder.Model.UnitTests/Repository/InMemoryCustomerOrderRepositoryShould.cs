namespace CustomerOrder.Model.UnitTests.Repository
{
    using System;
    using Model.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class InMemoryCustomerOrderRepositoryShould
    {
        private InMemoryCustomerOrderRepository _repositoryUnderTest;
        private Mock<ICustomerOrderFactory> _customerOrderFactoryMock;

        [SetUp]
        public void SetUp()
        {
            _customerOrderFactoryMock = new Mock<ICustomerOrderFactory>();
            _repositoryUnderTest = new InMemoryCustomerOrderRepository(_customerOrderFactoryMock.Object, Currency.CHF);
        }

        [Test]
        public void CreateANewOrderIfTheSpecifiedOrderIdentifierDoesNotExist()
        {
            var expectedOrder = new Mock<ICustomerOrder>().Object;
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(orderIdentifier, Currency.CHF)).Returns(expectedOrder);

            var actualOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier).Result;
            Assert.AreSame(expectedOrder, actualOrder);
        }

        [Test]
        public void ReturnTheSameOrderIfTheSpecifiedOrderIdentifierIsUsedAgain()
        {
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(orderIdentifier, Currency.CHF)).Returns(() => new Mock<ICustomerOrder>().Object);

            var expectedOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier).Result;
            var actualOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier).Result;

            Assert.AreSame(expectedOrder, actualOrder);
        }

    }
}

