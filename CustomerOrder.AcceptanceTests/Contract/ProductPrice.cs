namespace CustomerOrder.AcceptanceTests.Contract
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly]
    internal class ProductPrice
    {
        [JsonProperty]
        public Money Unit { get; set; }
        [JsonProperty]
        public Money Net { get; set; }
    }
}