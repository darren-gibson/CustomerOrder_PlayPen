namespace CustomerOrder.AcceptanceTests.Contract
{
    using Newtonsoft.Json;

    internal class OrderTotals
    {
        [JsonProperty(PropertyName = "net")]
        public Money Net { get; set; }
        [JsonProperty(PropertyName = "due")]
        public Money Due { get; set; }
        [JsonProperty(PropertyName = "paid")]
        public Money Paid { get; set; }
    }
}