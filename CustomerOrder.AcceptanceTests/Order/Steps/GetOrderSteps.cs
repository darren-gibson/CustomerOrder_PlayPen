﻿namespace CustomerOrder.AcceptanceTests.Steps
{
    using System.Linq;
    using System.Threading;
    using Contract;
    using Helpers;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using ProductAdd.Steps;
    using TechTalk.SpecFlow;

    using CustomerOrder = Contract.CustomerOrder;
    using Quantity = Contract.Quantity;

    [Binding]
    [Scope(Feature = "Get Order")]
    public class GetOrderStep : FeatureBase
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

        [When(@"I GET (.*) with an accept header of (.*)")]
        public void WhenIGetOrderWithTheOrderNo(string relativeUrl, string acceptHeader)
        {
            Result = Client.GetOrder(ReplaceTokensInString(relativeUrl), acceptHeader);
        }

        [Then(@"the result should be an HTTP (.*) Status")]
        public void ThenTheResultShouldBeAnHttpOkStatus(string expectedStatus)
        {
            Client.AssertStatusAreEqual(Result, expectedStatus);
        }


        [Then(@"the order should contain the following products:")]
        public void ThenTheOrderShouldContain(Table table)
        {
            var products = GetOrderFromResult().Products;
            foreach (var tableRow in table.Rows)
            {
                var expectedProduct = tableRow["Product"];
                var expectedQuantity = Quantity.Parse(tableRow["Quantity"]);

                var product = products.FirstOrDefault(p => p.ProductId.Equals(expectedProduct));

                Assert.NotNull(product, string.Format("Expected Products to collection contain {0}", expectedProduct));
                Assert.NotNull(product.Quantity, "Quantity expected");
                Assert.AreEqual(expectedQuantity.Amount, product.Quantity.Amount);
                Assert.AreEqual(expectedQuantity.UOM, product.Quantity.UOM);

                CompareProductPriceIfRequestedInTable(tableRow, product);
            }
        }

        private static void CompareProductPriceIfRequestedInTable(TableRow tableRow, Product product)
        {
            if (tableRow.ContainsKey("Unit Price"))
            {
                var expectedUnitPrice = ToMoney(tableRow["Unit Price"]);
                Assert.AreEqual(expectedUnitPrice, ToMoney(product.Price.Unit), "Invalid Unit Price");
            }
            if (tableRow.ContainsKey("Net Price"))
            {
                var expectedNetPrice = ToMoney(tableRow["Net Price"]);
                Assert.AreEqual(expectedNetPrice, ToMoney(product.Price.Net), "Invalid Net Price");
            }
        }

        [Then(@"the order should have a (.*) of (.*)")]
        public void ThenTheOrderShouldHaveANetTotalOf(string totalName, string expectedAmountString)
        {
            var expectedAmount = ToMoney(expectedAmountString);
            var order = GetOrderFromResult();
            switch (totalName)
            {
                case "netTotal":
                    Assert.AreEqual(expectedAmount, ToMoney(order.Total.Net));
                    break;
                default:
                    Assert.Fail("unknown total field");
                    break;
            }
        }

        private static Money ToMoney(Price price)
        {
            return new Money(ToCurrency(price.CurrencyCode), price.Amount);
        }
        private CustomerOrder GetOrderFromResult()
        {
            var content = Result.Content;
            var jsonContent = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<CustomerOrder>(jsonContent);
        }
    }
}