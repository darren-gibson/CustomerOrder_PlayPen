﻿namespace CustomerOrder.PriceServiceStub.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SimplePriceGatewayShould
    {
        private SimplePriceGateway _priceUnderTest;

        [SetUp]
        public void SetUp()
        {
            _priceUnderTest = new SimplePriceGateway();
        }

        [Test]
        public void ReturnAPricedOrderThatContainsAPriceForEachProductInTheOrder()
        {
            var products = SetupProducts(new decimal[] { 3, 4, 5 }).ToArray();
            var mockOrder = new Mock<ICustomerOrder>();
            mockOrder.SetupGet(o => o.Products).Returns(products);

            var pricedOrder = _priceUnderTest.Price(mockOrder.Object);

            Assert.AreEqual(GetPrice(3), pricedOrder.GetProductPrice(products[0]).UnitPrice);
            Assert.AreEqual(GetPrice(4), pricedOrder.GetProductPrice(products[1]).UnitPrice);
            Assert.AreEqual(GetPrice(5), pricedOrder.GetProductPrice(products[2]).UnitPrice);
            Assert.AreEqual(GetPrice(3), pricedOrder.GetProductPrice(products[0]).NetPrice);
            Assert.AreEqual(GetPrice(4), pricedOrder.GetProductPrice(products[1]).NetPrice);
            Assert.AreEqual(GetPrice(5), pricedOrder.GetProductPrice(products[2]).NetPrice);
        }

        [Test]
        public void CorrectlyPriceTheNetPriceBasedOnTheQuantity()
        {
            var product = SetupProductAndSetPrice(1.20m, 3);
            var mockOrder = new Mock<ICustomerOrder>();
            mockOrder.SetupGet(o => o.Products).Returns(new[] { product });

            var pricedOrder = _priceUnderTest.Price(mockOrder.Object);

            Assert.AreEqual(GetPrice(1.2m), pricedOrder.GetProductPrice(product).UnitPrice);
            Assert.AreEqual(GetPrice(3.6m), pricedOrder.GetProductPrice(product).NetPrice);
        }

        [Test]
        public void ReturnTheSumOfAllOfTheProductsInTheOrderAsTheNetAmount()
        {
            var products = SetupProducts(new[] { 3, 4.2m, 5 }, 3).ToArray();
            var mockOrder = new Mock<ICustomerOrder>();
            mockOrder.SetupGet(o => o.Products).Returns(products);

            var pricedOrder = _priceUnderTest.Price(mockOrder.Object);

            var expected = new Money(Currency.GBP, 12.2m*3m);
            var actual = pricedOrder.NetTotal;

            Assert.IsTrue(expected.Equals(actual), "Expected={0}, actual={1}", expected, actual);
        }

        [Test]
        public void ThrowsAProductNotFoundExceptionWhenAnInvalidProductIsPassed()
        {
            var mockOrder = new Mock<ICustomerOrder>();
            mockOrder.SetupGet(o => o.Products).Returns(new[] {CreateProductMock(Guid.NewGuid(), 1.2m).Object});

            Assert.Throws<ProductNotFoundException>(() => _priceUnderTest.Price(mockOrder.Object));
        }

        private Money GetPrice(decimal price)
        {
            return new Money(Currency.GBP, price);
        }

        private IEnumerable<IProduct> SetupProducts(IList<decimal> prices, decimal quantityInOrder = 1m)
        {
            for (var i = 0; i < prices.Count(); i++)
            {
                yield return SetupProductAndSetPrice(prices[i], quantityInOrder);
            }
        }

        private IProduct SetupProductAndSetPrice(decimal price, decimal quantityInOrder = 1m)
        {
            var productIdentifier = Guid.NewGuid();
            var productMock = CreateProductMock(productIdentifier, quantityInOrder);
            var quantityPrice = GetQuantityPrice(price);
            _priceUnderTest.SetPrice(productIdentifier, quantityPrice);
            return productMock.Object;
        }

        private static Mock<IProduct> CreateProductMock(Guid productIdentifier, decimal quantityInOrder)
        {
            var productMock = new Mock<IProduct>();
            productMock.SetupGet(p => p.ProductIdentifier).Returns(productIdentifier);
            productMock.SetupGet(p => p.Quantity).Returns(new Quantity(quantityInOrder, UnitOfMeasure.Each));
            return productMock;
        }

        private static QuantityPrice GetQuantityPrice(decimal price)
        {
            return new QuantityPrice(new Money(Currency.GBP, price), 1);
        }
    }
}
