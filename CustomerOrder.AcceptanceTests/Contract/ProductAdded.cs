namespace CustomerOrder.AcceptanceTests.Contract
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class ProductAdded
    {
        [JsonProperty(PropertyName = "productId")]
        public string ProductIdentifier { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public Quantity Quantity { get; set; }        
        [JsonProperty(PropertyName = "price")]
        public ProductPrice Price { get; set; }
    }
}
