namespace CustomerOrder.Contract.DTO
{
    using Annotations;
    using Newtonsoft.Json;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PaymentExceededAmountDueException
    {
        public PaymentExceededAmountDueException(Model.PaymentExceededAmountDueException exception)
        {
            AmountDue = new Money(exception.AmountDue);
        }

        [JsonProperty(PropertyName = "amountDue")]
        public Money AmountDue { get; set; }
    }
}