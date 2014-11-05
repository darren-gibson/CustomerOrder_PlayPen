namespace CustomerOrder.AcceptanceTests.Steps
{
    using System;
    using System.Linq;
    using System.Threading;
    using Helpers;
    using NUnit.Framework;
    using ProductAdd.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "All Events for an Order")]
    public class AllEventsForAnOrderSteps : FeatureBase
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

        [Then(@"the result should be an HTTP (.*) Status")]
        public void ThenTheResultShouldBeAnHttpStatus(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }

        [When(@"I GET (.*) with an Accept header of (.*)")]
        public void WhenIGetWithAnAcceptHeaderOf(string url, string acceptHeader)
        {
            Thread.Sleep(100); // Allow the Asynchronous event to happen
            Result = Client.GetAllEvents(ReplaceTokensInString(url), acceptHeader);
        }

        [Then(@"the result should contain no events")]
        public void ThenTheResultShouldContainNoEvents()
        {
            var feed = ReadSyndicationFeed();
            var syndicationItem = feed.Items.FirstOrDefault();
            Assert.IsNull(syndicationItem);
        }

        [Given(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)""")]
        [When(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)""")]
        public void WhenIAddAProductToANewOrderByCallingPOSTOrdersProductaddWith(string url, string productId)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId);
        }

        [Then(@"the result should contain (.*) events of type:")]
        public void ThenTheResultShouldEventsOfType(int numberOfEvents, Table table)
        {
            var feed = ReadSyndicationFeed();
            Assert.AreEqual(numberOfEvents, feed.Items.Count());
        }

    }
}
