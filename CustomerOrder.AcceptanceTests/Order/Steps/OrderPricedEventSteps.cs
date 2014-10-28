namespace CustomerOrder.AcceptanceTests.Steps
{
    using System;
    using System.Threading;
    using Contract;
    using Helpers;
    using Model;
    using NUnit.Framework;
    using ProductAdd.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "Order Priced Event")]
    public class OrderPricedEventSteps : FeatureBase
    {
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

        [Given(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)""")]
        [When(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)""")]
        public void WhenIAddAProductToANewOrderByCallingPOSTOrdersProductaddWith(string url, string productId)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId);
        }


        [Then(@"the result should be an HTTP (.*) Status")]
        public void ThenTheResultShouldBeAnHTTPOKStatus(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }

        [When(@"I GET (.*) with an Accept header of (.*)")]
        public void WhenIGETWithAnAcceptHeaderOf(string url, string acceptHeader)
        {
            Thread.Sleep(50); // Allow the Async event to happen
            Result = Client.GetOrderPricedEvents(ReplaceTokensInString(url), acceptHeader);
        }

        [Then(@"the result should contain:")]
        public void ThenTheResultShouldContain(Table table)
        {
            var orderPricedEvent = GetEventFromFirstSyndicationItem<OrderPricedEvent>();

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                var expectedValue = ReplaceTokensInString(tableRow["Value"]);

                switch (name)
                {
                    case "order":
                        Assert.AreEqual(expectedValue, orderPricedEvent.Order);
                        break;
                    case "netTotal":
                        var expected = ToMoney(expectedValue);
                        Assert.AreEqual(expected,
                            new Money(ToCurrency(orderPricedEvent.NetTotal.CurrencyCode),
                                orderPricedEvent.NetTotal.Amount));
                        break;
                    default:
                        Assert.Fail("Unknown field: {0}", name);
                        break;
                }
            }
        }
    }
}
