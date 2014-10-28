namespace CustomerOrder.AcceptanceTests.Contract
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly]
    internal class ProductPrice
    {
        [JsonProperty]
        public Price Unit { get; set; }
        [JsonProperty]
        public Price Net { get; set; }
    }
}