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
            _repositoryUnderTest = new InMemoryCustomerOrderRepository(_customerOrderFactoryMock.Object);
        }

        [Test]
        public void CreateANewOrderIfTheSpecifiedOrderIdentifierDoesNotExist()
        {
            var expectedOrder = new Mock<ICustomerOrder>().Object;
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(orderIdentifier)).Returns(expectedOrder);

            var actualOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier);
            Assert.AreSame(expectedOrder, actualOrder);
        }

        [Test]
        public void ReturnTheSameOrderIfTheSpecifiedOrderIdentifierIsUsedAgain()
        {
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(orderIdentifier)).Returns(() => new Mock<ICustomerOrder>().Object);

            var expectedOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier);
            var actualOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier);

            Assert.AreSame(expectedOrder, actualOrder);
        }

    }
}

