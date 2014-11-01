namespace CustomerOrder.AcceptanceTests.Contract
{
    using Newtonsoft.Json;

    internal class OrderTotals
    {
        [JsonProperty(PropertyName = "net")]
        public Money Net { get; set; }
    }
}