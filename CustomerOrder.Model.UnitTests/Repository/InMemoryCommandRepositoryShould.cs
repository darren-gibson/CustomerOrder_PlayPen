namespace CustomerOrder.Model.UnitTests.Repository
{
    using System;
    using Model.Command;
    using Model.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class InMemoryCommandRepositoryShould
    {
        private InMemoryCommandRepository _repositoryUnderTest;

        [SetUp]
        public void SetUp()
        {
            _repositoryUnderTest = new InMemoryCommandRepository();
        }

        [Test]
        public void ThrowANotFoundExceptionIfTheCommandIdentifierIsNotFound()
        {
            Assert.Throws<NotFoundException>(() => _repositoryUnderTest.GetCommandResultById(Guid.NewGuid()));
        }

        [Test]
        public void ReturnTheResponseIfTheResponseIsFound()
        {
            var command = GetCommand();
            var expectedResult = new object();
            _repositoryUnderTest.Save(command, expectedResult);
            var result = _repositoryUnderTest.GetCommandResultById(command.Id);
            Assert.AreSame(expectedResult, result);
        }

        private ICommand GetCommand()
        {
            var commandMock = new Mock<ICommand>();
            commandMock.SetupGet(c => c.Id).Returns(Guid.NewGuid());
            return commandMock.Object;
        }

        [Test]
        public void ReturnNullIfThereIsNoResultButTheCommandIsKnow()
        {
            var command = GetCommand();
            _repositoryUnderTest.Save(command);
            var result = _repositoryUnderTest.GetCommandResultById(command.Id);
            Assert.IsNull(result);
        }

        [Test]
        public void ThrowAnArgumentNullExceptionIfTheResultIsAttemptedToBeSavedAsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repositoryUnderTest.Save(GetCommand(), null));
        }

/*
        [Test]
        public void ReturnTheSameOrderIfTheSpecifiedOrderIdentifierIsUsedAgain()
        {
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            _customerOrderFactoryMock.Setup(f => f.MakeCustomerOrder(orderIdentifier)).Returns(() => new Mock<ICustomerOrder>().Object);

            var expectedOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier);
            var actualOrder = _repositoryUnderTest.GetOrCreateOrderById(orderIdentifier);

            Assert.AreSame(expectedOrder, actualOrder);
        } */
    }
}