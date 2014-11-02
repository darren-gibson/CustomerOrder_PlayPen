namespace CustomerOrder.AcceptanceTests.Contract
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class PaymentExceededAmountDueException
    {
        [JsonProperty(PropertyName = "amountDue")]
        public Money AmountDue { get; set; }
    }
}