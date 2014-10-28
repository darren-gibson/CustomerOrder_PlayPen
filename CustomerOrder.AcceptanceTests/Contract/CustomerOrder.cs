namespace CustomerOrder.AcceptanceTests.Contract
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal class CustomerOrder
    {
        [JsonProperty(PropertyName= "products")]
        public List<Product> Products { get; set; }

        [JsonProperty(PropertyName = "total")]
        public OrderTotals Total { get; set; }
    }
}