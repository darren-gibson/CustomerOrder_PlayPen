namespace CustomerOrder.AcceptanceTests.Contract
{
    using Newtonsoft.Json;

    internal class OrderTotals
    {
        [JsonProperty(PropertyName = "net")]
        public Price Net { get; set; }
    }
}