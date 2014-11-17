namespace CustomerOrder.Model.UnitTests.Command
{
    using System;
    using System.Threading.Tasks;
    using Model.Command;
    using Model.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProductAddCommandShould
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
        public void GetTheOrderFromTheRepositoryAndCallProductAddWithTheDefaultQuantity()
        {
            ProductIdentifier expectedProductId = Guid.NewGuid();
            var expectedQuantity = new Quantity(1, UnitOfMeasure.Each);
            var commandUnderTest = new ProductAddCommand(_orderRepositoryMock.Object, _orderIdentifier, expectedProductId);

            commandUnderTest.Execute();

            _orderMock.Verify(o => o.ProductAdd(expectedProductId, expectedQuantity), Times.Once);
        }

        [Test]
        public void GetTheOrderFromTheRepositoryAndCallProductAddWithTheGivenQuantity()
        {
            ProductIdentifier expectedProductId = Guid.NewGuid();
            var expectedQuantity = new Quantity(3, UnitOfMeasure.ML);
            var commandUnderTest = new ProductAddCommand(_orderRepositoryMock.Object, _orderIdentifier,
                expectedProductId, expectedQuantity);

            commandUnderTest.Execute();

            _orderMock.Verify(o => o.ProductAdd(expectedProductId, expectedQuantity), Times.Once);
        }

        [Test]
        public void SetTheIdWhenPassedViaTheConstructor()
        {
            var expectedId = Guid.NewGuid();
            var commandUnderTest = new ProductAddCommand(_orderRepositoryMock.Object, _orderIdentifier, Guid.NewGuid(),
                id: expectedId);
            Assert.AreEqual(expectedId, commandUnderTest.Id);
        }

        [Test]
        public void CreateAnIdIfOneIsNotSupplied()
        {
            var commandUnderTest = new ProductAddCommand(_orderRepositoryMock.Object, _orderIdentifier, Guid.NewGuid());
            Assert.AreNotEqual(Guid.Empty, commandUnderTest.Id);
        }

        [Test]
        public void ReturnsTheProductAddedItemWhenSucceeds()
        {
            var quantity = new Quantity(3.3m, UnitOfMeasure.ML);
            ProductIdentifier productIdentifier = Guid.NewGuid();
            var expectedResult = new ProductAdded(null, null);

            _orderMock.Setup(o => o.ProductAdd(productIdentifier, quantity)).Returns(expectedResult);

            var commandUnderTest = new ProductAddCommand(_orderRepositoryMock.Object, _orderIdentifier, productIdentifier, quantity);
            var result = commandUnderTest.Execute();
            Assert.AreSame(expectedResult, result);
        }
    }
}