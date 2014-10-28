namespace CustomerOrder.ProductServiceStub
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal class SearchResult
    {
        public SearchResult()
        {
            Products = new List<Product>();
            MissingSet = new List<string>();
        }

        [JsonProperty(PropertyName = "products")]
        public List<Product> Products { get; private set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get { return Products.Count; } }

        [JsonProperty(PropertyName = "missingSet")]
        public List<string> MissingSet { get; private set; }
    }
}
