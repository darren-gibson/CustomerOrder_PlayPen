using NUnit.Framework;

namespace CustomerOrder.ProductServiceStub.UnitTests
{
    using System.IO;

    [TestFixture]
    public class SimpleProductServiceShould
    {
        private SimpleProductService _serviceUnderTest;

        [SetUp]
        public void SetUp()
        {
            _serviceUnderTest = new SimpleProductService();
        }
        [Test]
        public void ReturnAJsonProductItemInTheCorrectFormat()
        {
            const string productId = "55";
            const string description = "Lemons";
            const string imageUrl = "http://some/url/lemons";

            _serviceUnderTest.AddProduct(productId, description, imageUrl);
            var result = ExecuteSerializeAndGetResultAsString(productId);
            Assert.AreEqual(GetExpectedJson(productId, description, imageUrl), result);

        }

        private string ExecuteSerializeAndGetResultAsString(string productId)
        {
            using (var ms = new MemoryStream())
            {
                _serviceUnderTest.Serialize(new[] { productId }, new NonClosingStream(ms));
                ms.Position = 0;
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }

        [Test]
        public void ReturnANonFoundResultSetWhenSearchingForAnUnknownProduct()
        {
            const string missingSetResult = @"{""products"":[],""total"":0,""missingSet"":[""urn:epc:id:gtin:00000000000055""]}";
            var result = ExecuteSerializeAndGetResultAsString("urn:epc:id:gtin:00000000000055");
            Assert.AreEqual(missingSetResult, result);
        }

        private string GetExpectedJson(string productId, string description, string imageUrl)
        {
            return @"{""products"":[{""description"":""" + description + @""",""imageUrl"":[""" + imageUrl + @"""],""GTIN"":[""" + productId + @"""]}],""total"":1,""missingSet"":[]}";
        }
    }
}

