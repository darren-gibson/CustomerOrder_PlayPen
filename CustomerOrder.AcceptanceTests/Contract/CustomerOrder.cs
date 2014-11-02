namespace CustomerOrder.AcceptanceTests.Contract
{
    using System.Collections.Generic;
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly]
    internal class CustomerOrder
    {
        [JsonProperty(PropertyName= "products")]
        public List<Product> Products { get; set; }
        [JsonProperty(PropertyName = "payments")]
        public List<Payment> Payments { get; set; }
        [JsonProperty(PropertyName = "total")]
        public OrderTotals Total { get; set; }
    }
}