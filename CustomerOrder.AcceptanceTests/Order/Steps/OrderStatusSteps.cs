namespace CustomerOrder.AcceptanceTests.Order.Steps
{
    using System;
    using System.Linq;
    using System.Threading;
    using Contract;
    using Helpers;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using CustomerOrder = Contract.CustomerOrder;
    using Money = Contract.Money;
    using Quantity = Contract.Quantity;

    [Binding]
    [Scope(Feature = "Order Status")]
    public class OrderStatusSteps : ProductAdd.Steps.FeatureBase
    {
        [When(@"I GET (.*) with an Accept header of (.*)")]
        public void WhenIGetWithAnAcceptHeaderOf(string url, string acceptHeader)
        {
            Thread.Sleep(50); // Allow the asynchronous event to happen
            Result = Client.GetOrderPricedEvents(ReplaceTokensInString(url), acceptHeader);
        }

        [Given(@"the following products are know to the Price Service:")]
        public void GivenTheFollowingProductsAreKnowToThePriceService(Table table)
        {
            var priceSetup = new PriceSetup();
            priceSetup.Setup(table);
        }

        [Given(@"I have a unique order number (.*)")]
        public void GivenIHaveAUniqueOrderNumber(string token)
        {
            OrderNumberToken = token;
            // TODO: this should not include a colon, the format is not a valid URL
            OrderNumber = string.Format("trn:tesco:order:uuid:{0}", Guid.NewGuid());
            Client = new CustomerOrderHttpClient();
        }

        [Given(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)""")]
        [When(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)""")]
        public void WhenIAddAProductToANewOrderByCallingPostWithAProductId(string url, string productId)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId);
        }

        [When(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)"" with a Quantity of ""(.*) (.*)""")]
        public void WhenIAddAProductToANewOrderByCallingPostWithAProductId(string url, string productId, decimal amount, UnitOfMeasure unitOfMeasure)
        {
            var quantity = new Model.Quantity(amount, unitOfMeasure);
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId, quantity);
        }

        [When(@"I add a Payment to an order by calling PUT (.*) with a tender type of (.*) and an amount of (.*) (.{3})")]
        public void WhenIAddAPaymentToAnOrderByCallingPutOrdersOrderNoTendersWithATenderTypeOfAndAnAmountOf(string url, string tenderType, decimal amount, Currency currency)
        {
            Result = Client.PaymentAdd(ReplaceTokensInString(url), tenderType, new Model.Money(currency, amount));
        }


        [When(@"I GET (.*) with an accept header of (.*)")]
        public void WhenIGetOrderWithTheOrderNo(string relativeUrl, string acceptHeader)
        {
            WaitForAllCommandsToHaveCompleted();
            Result = Client.GetOrder(ReplaceTokensInString(relativeUrl), acceptHeader);
        }

        private void WaitForAllCommandsToHaveCompleted()
        {
            Thread.Sleep(50);
        }

        [Then(@"the result should be an HTTP (.*) Status")]
        public void ThenTheResultShouldBeAnHttpOkStatus(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }

        private CustomerOrder GetOrderFromResult()
        {
            var content = Result.Content;
            var jsonContent = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<CustomerOrder>(jsonContent);
        }

        [Then(@"the order status should be '(.*)'")]
        public void ThenTheOrderStatusShouldBe(string expectedStatus)
        {
            var order = GetOrderFromResult();
            Assert.AreEqual(expectedStatus, order.Status);
        }

    }
}
