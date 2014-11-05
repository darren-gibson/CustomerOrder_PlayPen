namespace CustomerOrder.AcceptanceTests.Order.Steps
{
    using System;
    using System.Threading;
    using Helpers;
    using Model;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "Completed Order Cannot Be Changed")]
    public class CompletedOrderCannotBeChangedSteps : ProductAdd.Steps.FeatureBase
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

        [Given(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)""")]
        [When(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)""")]
        public void WhenIAddAProductToANewOrderByCallingPostWithAProductId(string url, string productId)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId);
        }


        [Given(@"I add a Payment to an order by calling PUT (.*) with a tender type of (.*) and an amount of (.*) (.{3})")]
        public void WhenIAddAPaymentToAnOrderByCallingPutOrdersOrderNoTendersWithATenderTypeOfAndAnAmountOf(string url, string tenderType, decimal amount, Currency currency)
        {
            Result = Client.PaymentAdd(ReplaceTokensInString(url), tenderType, new Model.Money(currency, amount));
        }

        [When(@"I GET the resource identified by the Uri in the Location Header with an Accept header of (.*)")]
        public void WhenIGetTheResourceIdentifiedByTheUriInTheLocationHeaderWithAnAcceptHeaderOf(string acceptHeader)
        {
            WaitForAllCommandsToHaveCompleted();
            var url = Result.Headers.Location;
            Result = Client.GetUrl(url.ToString(), acceptHeader);
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

        [Then(@"the result should have a Content-Type of '(.*)'")]
        public void ThenTheResultShouldHaveAContent_TypeOf(string expectedContentType)
        {
            Assert.AreEqual(expectedContentType, Result.Content.Headers.ContentType.MediaType);
        }
    }
}
