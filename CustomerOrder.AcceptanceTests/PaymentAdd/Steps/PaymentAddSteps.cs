namespace CustomerOrder.AcceptanceTests.PaymentAdd.Steps
{
    using System;
    using Helpers;
    using Model;
    using NUnit.Framework;
    using ProductAdd.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "Payment Add")]
    public class TenderAddSteps : FeatureBase
    {
        [Given(@"I have a unique order number (.*)")]
        public void GivenIHaveAUniqueOrderNumber(string token)
        {
            OrderNumberToken = token;
            // TODO: this should not include a colon, the format is not a valid URL
            OrderNumber = string.Format("trn:tesco:order:uuid:{0}", Guid.NewGuid());
            Client = new CustomerOrderHttpClient();
        }

        [When(@"I add a Payment to an order by calling PUT (.*) with a tender type of (.*) and an amount of (.*) (.{3})")]
        public void WhenIAddAPaymentToAnOrderByCallingPutOrdersOrderNoTendersWithATenderTypeOfAndAnAmountOf(string url, string tenderType, decimal amount, Currency currency)
        {
            Result = Client.PaymentAdd(ReplaceTokensInString(url), tenderType, new Money(currency, amount));            
        }

        [Then(@"the result should be an HTTP (.*) Status")]
        public void ThenTheResultShouldBeAnHttpStatusOf(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }

        [Then(@"the result should contain a Location header that matches (.*)")]
        public void ThenTheResultShouldContainALocationHeaderThatMatchesTheMask(string expectedMask)
        {
            Assert.IsNotNull(Result.Headers.Location, "Expected a Location header in the response");
            expectedMask = ReplaceTokensInString(expectedMask);
            StringAssert.IsMatch(expectedMask, Result.Headers.Location.ToString());
        }
    }
}
