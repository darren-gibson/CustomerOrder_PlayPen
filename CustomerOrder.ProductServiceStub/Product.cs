namespace CustomerOrder.ProductServiceStub
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal class Product
    {
        public Product(string productId, string description, string imageUrl)
        {
            Description = description;
            ImageUrl = new List<string>(new[] { imageUrl });
            GlobalTrageItemNumbers = new List<string>(new[] { productId });
        }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "imageUrl")]
        public List<string> ImageUrl { get; private set; }

        [JsonProperty(PropertyName = "GTIN")]
        public List<string> GlobalTrageItemNumbers { get; private set; }
    }
}
