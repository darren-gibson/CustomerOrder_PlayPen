namespace CustomerOrder.Model.UnitTests.Order
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderLifecycleShould : CustomerOrderSetupShould
    {
        [Test]
        public void HaveAnInitialStatusOfNotStarted()
        {
            Assert.AreEqual(CustomerOrderStatus.NotStarted, OrderUnderTest.Status);
        }

        [Test]
        public void HaveAStatusOfShoppingWhenTheOrderHasJustOneProductInIt()
        {
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            Assert.AreEqual(CustomerOrderStatus.Shopping, OrderUnderTest.Status);
        }

        [Test]
        public void StayInShoppingWhenAddingMoreProductsToTheOrder()
        {
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            Assert.AreEqual(CustomerOrderStatus.Shopping, OrderUnderTest.Status);
        }

        [Test]
        public void HaveAStatusOfCompleteWhenTheOrderHasBeenFullyPaidFor()
        {
            var totalOrderAmount = SetTotalOrderAmount(10m);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.PaymentAdd(new Tender(totalOrderAmount, "Cash"));

            Assert.AreEqual(CustomerOrderStatus.Complete, OrderUnderTest.Status);
        }

        private Money SetTotalOrderAmount(decimal amount)
        {
            var totalOrderAmount = new Money(OrderUnderTest.Currency, amount);
            PricedOrderMock.SetupGet(p => p.NetTotal).Returns(totalOrderAmount);
            return totalOrderAmount;
        }

        [Test]
        public void HaveAStatusOfPayingWhenOnlyPartPaymentIsReceived()
        {
            SetTotalOrderAmount(10m);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.PaymentAdd(new Tender(new Money(OrderUnderTest.Currency, 1m), "Cash"));

            Assert.AreEqual(CustomerOrderStatus.Paying, OrderUnderTest.Status);
        }

        [Test]
        public void HaveAStatusOfCompleteWhenMultiplePaymentsAddUpToTheFullPayment()
        {
            SetTotalOrderAmount(10m);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.PaymentAdd(new Tender(new Money(OrderUnderTest.Currency, 1m), "Cash"));
            OrderUnderTest.PaymentAdd(new Tender(new Money(OrderUnderTest.Currency, 9m), "Cash"));

            Assert.AreEqual(CustomerOrderStatus.Complete, OrderUnderTest.Status);
        }

        [Test]
        public void LeaveTheOrderInPayingWhenMultiplePartPaymentsAreReceived()
        {
            SetTotalOrderAmount(10m);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.PaymentAdd(new Tender(new Money(OrderUnderTest.Currency, 1m), "Cash"));
            OrderUnderTest.PaymentAdd(new Tender(new Money(OrderUnderTest.Currency, 1m), "Cash"));

            Assert.AreEqual(CustomerOrderStatus.Paying, OrderUnderTest.Status);
        }

        [Test]
        public void CannotAddProductToACompletedOrder()
        {
            var totalOrderAmount = SetTotalOrderAmount(10m);
            OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default);
            OrderUnderTest.PaymentAdd(new Tender(totalOrderAmount, "Cash"));
            Assert.Throws<InvalidOperationException>(() => OrderUnderTest.ProductAdd(Guid.NewGuid(), Quantity.Default));
        }
    }
}