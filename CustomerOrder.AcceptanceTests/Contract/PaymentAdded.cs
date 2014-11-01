namespace CustomerOrder.AcceptanceTests.Contract
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class PaymentAdded
    {
        [JsonProperty(PropertyName = "tenderType")]
        public string TenderType { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; set; }
    }
}
