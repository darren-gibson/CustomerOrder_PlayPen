namespace CustomerOrder.Model.UnitTests
{
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class OrderPricedEventArgsShould
    {
        [Test]
        public void ContainTheInformationPassedInTheConstructor()
        {
            var pricedOrder = new Mock<IPricedOrder>().Object;
            var eventArgsUnderTest = new OrderPricedEventArgs(pricedOrder);

            Assert.AreEqual(pricedOrder, eventArgsUnderTest.PricedOrder);
        } 
    }
}