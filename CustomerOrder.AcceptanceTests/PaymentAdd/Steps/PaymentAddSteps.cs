namespace CustomerOrder.AcceptanceTests.PaymentAdd.Steps
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using Helpers;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using ProductAdd.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "Payment Add")]
    public class PaymentAddSteps : FeatureBase
    {
        [Given(@"I have a unique order number (.*)")]
        public void GivenIHaveAUniqueOrderNumber(string token)
        {
            OrderNumberToken = token;
            // TODO: this should not include a colon, the format is not a valid URL
            OrderNumber = string.Format("trn:tesco:order:uuid:{0}", Guid.NewGuid());
            Client = new CustomerOrderHttpClient();
        }

        [Given(@"the following products are know to the Price Service:")]
        public void GivenTheFollowingProductsAreKnowToThePriceService(Table table)
        {
            var priceSetup = new PriceSetup();
            priceSetup.Setup(table);
        }

        [Given(@"I add a Product to the order by calling POST (.*) with a ProductID of ""(\d+)"" with a Quantity of ""(.*) (.*)""")]
        public void WhenIAddAProductToANewOrderByCallingPostWithAProductId(string url, string productId, decimal amount, UnitOfMeasure unitOfMeasure)
        {
            var quantity = new Model.Quantity(amount, unitOfMeasure);
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId, quantity);
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

        [When(@"I GET the resource identified by the Uri in the Location Header with an Accept header of (.*)")]
        public void WhenIGetTheResourceIdentifiedByTheUriInTheLocationHeaderWithAnAcceptHeaderOf(string acceptHeader)
        {
            Thread.Sleep(50); // Allow the Asynchronous event to happen
            var url = Result.Headers.Location;
            Result = Client.GetUrl(url.ToString(), acceptHeader);
        }

        [Then(@"the result should contain:")]
        public void ThenTheResultShouldContain(Table table)
        {
            var paymentAdded = GetPaymentAdded(Result);

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                var expectedValue = ReplaceTokensInString(tableRow["Value"]);

                switch (name)
                {
                    case "tenderType":
                        Assert.AreEqual(expectedValue, paymentAdded.TenderType);
                        break;
                    case "amount":
                        AssertMoneyEqual(expectedValue, paymentAdded.Amount);
                        break;
                    default:
                        Assert.Fail("Unknown field: {0}", name);
                        break;
                }
            }
        }

        private Contract.PaymentAdded GetPaymentAdded(HttpResponseMessage result)
        {
            return JsonConvert.DeserializeObject<Contract.PaymentAdded>(result.Content.ReadAsStringAsync().Result);
        }

        [Then(@"the result should have a Content-Type of '(.*)'")]
        public void ThenTheResultShouldHaveAContent_TypeOf(string expectedContentType)
        {
            Assert.AreEqual(expectedContentType, Result.Content.Headers.ContentType.MediaType);
        }

        [Then(@"the PaymentExceededAmountDueException should contain:")]
        public void ThenThePaymentExceededAmountDueExceptionShouldContain(Table table)
        {
            var paymentExceededAmountDueException = JsonConvert.DeserializeObject<Contract.PaymentExceededAmountDueException>(Result.Content.ReadAsStringAsync().Result);

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                var expectedValue = ReplaceTokensInString(tableRow["Value"]);

                switch (name)
                {
                    case "amountDue":
                        AssertMoneyEqual(expectedValue, paymentExceededAmountDueException.AmountDue);
                        break;
                    default:
                        Assert.Fail("Unknown field: {0}", name);
                        break;
                }
            }
        }
    }
}
