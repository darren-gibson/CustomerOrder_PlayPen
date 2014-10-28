namespace CustomerOrder.AcceptanceTests.ProductAdd.Steps
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using Helpers;
    using Model;
    using Model.Command;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using ProductAdded = Contract.ProductAdded;

    [Binding]
    [Scope(Feature = "Add Product")]
    public class AddProductSteps : FeatureBase
    {
        [Given(@"the following products are know to the Price Service:")]
        public void GivenTheFollowingProductsAreKnowToThePriceService(Table table)
        {
            var priceSetup = new PriceSetup();
            priceSetup.Setup(table);
        }

        [Given(@"the system is started")]
        public void GivenTheSystemIsStarted()
        {
            var commandRunner = Application.Registry.Resolve<CommandRunner>();
            commandRunner.Start();
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
        public void WhenIAddAProductToANewOrderByCallingPostOrdersProductAddWith(string url, string productId)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId);
        }

        [Given(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)"" and a Quantity of (.*) (.*)")]
        [When(@"I add a Product to a new order by calling POST (.*) with a ProductID of ""(.*)"" and a Quantity of (.*) (.*)")]
        public void WhenIAddAProductToANewOrderByCallingPostOrdersProductAddWithAQuantity(string url, string productId, decimal amount, UnitOfMeasure unitOfMeasure)
        {
            Result = Client.ProductAdd(ReplaceTokensInString(url), productId, new Quantity(amount, unitOfMeasure));
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

        [Given(@"I stop the service command runner")]
        public void GivenIStopTheServiceCommandRunner()
        {
            var commandRunner = Application.Registry.Resolve<CommandRunner>();
            commandRunner.Stop();
            while(commandRunner.Status != CommandRunnerStatus.Stopped)
                Thread.Sleep(50);
        }

        [When(@"I GET the resource identified by the Uri in the Location Header with an Accept header of (.*)")]
        public void WhenIGetTheResourceIdentifiedByTheUriInTheLocationHeaderWithAnAcceptHeaderOf(string acceptHeader)
        {
            Thread.Sleep(50); // Allow the Async event to happen
            var url = Result.Headers.Location;
            Result = Client.GetUrl(url.ToString(), acceptHeader);
        }

        [Then(@"the result should contain:")]
        public void ThenTheResultShouldContain(Table table)
        {
            var productAdded = GetProductAdded(Result);

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                var expectedValue = ReplaceTokensInString(tableRow["Value"]);

                switch (name)
                {
                    case "Content-Type":
                        Assert.AreEqual(expectedValue, Result.Content.Headers.ContentType.MediaType);
                        break;
                    case "productIdentifier":
                        Assert.AreEqual(expectedValue, productAdded.ProductIdentifier);
                        break;
                    case "quantity":
                        var parts = expectedValue.Split(' ');
                        Assert.AreEqual(decimal.Parse(parts[0]), productAdded.Quantity.Amount);
                        Assert.AreEqual(parts[1], productAdded.Quantity.UOM);
                        break;
                    case "unitPrice":
                        Assert.AreEqual(ToMoney(expectedValue), new Money(ToCurrency(productAdded.Price.Unit.CurrencyCode), productAdded.Price.Unit.Amount));
                        break;
                    case "netPrice":
                        Assert.AreEqual(ToMoney(expectedValue), new Money(ToCurrency(productAdded.Price.Net.CurrencyCode), productAdded.Price.Net.Amount));
                        break;
                    default:
                        Assert.Fail("Unknown field: {0}", name);
                        break;
                }
            }
        }

        private ProductAdded GetProductAdded(HttpResponseMessage result)
        {
            var content = result.Content;
            var jsonContent = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ProductAdded>(jsonContent);
        }
    }
}
