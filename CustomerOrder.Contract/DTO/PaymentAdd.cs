namespace CustomerOrder.Contract.DTO
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly]
    public class PaymentAdd
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }
        [JsonProperty(PropertyName = "tenderType")]
        public string TenderType { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set; }
    }
}
