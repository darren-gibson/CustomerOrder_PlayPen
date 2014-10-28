namespace CustomerOrder.AcceptanceTests.ProductAdd.Steps
{
    using System;
    using System.Linq;
    using System.Threading;
    using Helpers;
    using Model;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Feature = "Add Product Atom Events")]
    public class AddProductAtomEventSteps : FeatureBase
    {
        private string _savedId;


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
        public void ThenTheResultShouldBeAnHttpOkStatus(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }

        [When(@"I GET (.*) with an Accept header of (.*)")]
        public void WhenIGetWithAnAcceptHeaderOf(string url, string acceptHeader)
        {
            WaitForAsynchronousEventToHappen();
            Result = Client.GetProductAddedEvents(ReplaceTokensInString(url), acceptHeader);
        }

        private static void WaitForAsynchronousEventToHappen()
        {
            Thread.Sleep(50);
        }

        [Then(@"the result should contain:")]
        public void ThenTheResultShouldContain(Table table)
        {
            var productAddedEvent = GetEventFromFirstSyndicationItem<ProductAddedEvent>();

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                var expectedValue = ReplaceTokensInString(tableRow["Value"]);

                switch (name)
                {
                    case "product":
                        Assert.AreEqual(expectedValue, productAddedEvent.product);
                        break;
                    case "order":
                        Assert.AreEqual(expectedValue, productAddedEvent.order);
                        break;
                    case "quantity":
                        var parts = expectedValue.Split(' ');
                        Assert.AreEqual(decimal.Parse(parts[0]), productAddedEvent.quantity.Value);
                        Assert.AreEqual(parts[1], productAddedEvent.quantity.uom);
                        break;
                    case "unitPrice":
                        Assert.AreEqual(ToMoney(expectedValue), new Money(ToCurrency(productAddedEvent.price.unit.currency), productAddedEvent.price.unit.Value));
                        break;
                    case "netPrice":
                        Assert.AreEqual(ToMoney(expectedValue), new Money(ToCurrency(productAddedEvent.price.net.currency), productAddedEvent.price.net.Value));
                        break;
                    default:
                        Assert.Fail("Unknown field: {0}", name);
                        break;
                }
            }
        }

        [When(@"I save the Id of the productAdded event")]
        public void WhenISaveTheIdOfTheProductAddedEvent()
        {
            var item = GetFirstSyndicationItem();
            _savedId = item.Id;
        }

        [Then(@"the saved Id should equal the Id in the productAdded event just received")]
        public void ThenTheSavedIdShouldEqualTheIdInTheProductAddedEventJustReceived()
        {
            var item = GetFirstSyndicationItem();
            Assert.AreEqual(_savedId, item.Id);
        }

        [Then(@"the result should contain no events")]
        public void ThenTheResultShouldContainNoEvents()
        {
            var feed = ReadSyndicationFeed();
            var syndicationItem = feed.Items.FirstOrDefault();
            Assert.IsNull(syndicationItem);
        }
    }
}
