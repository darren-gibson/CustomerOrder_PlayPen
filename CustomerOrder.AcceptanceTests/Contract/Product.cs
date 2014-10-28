namespace CustomerOrder.AcceptanceTests.Contract
{
    using Newtonsoft.Json;

    internal class Product
    {
        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public Quantity Quantity { get; set; }

        public ProductPrice Price { get; set; }
    }
}