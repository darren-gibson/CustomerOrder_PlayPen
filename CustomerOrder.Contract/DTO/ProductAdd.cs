namespace CustomerOrder.Contract.DTO
{
    using Newtonsoft.Json;

    public class ProductAdd
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }
        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public Quantity Quantity { get; set; }
    }
}
