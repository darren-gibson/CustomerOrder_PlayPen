namespace CustomerOrder.Contract.DTO
{
    using Annotations;
    using Model;
    using Newtonsoft.Json;

    [UsedImplicitly]
    public class Payment
    {
        private readonly IPayment _payment;

        public Payment(IPayment payment)
        {
            _payment = payment;
        }

        [JsonProperty(PropertyName = "tenderType")]
        public string TenderType { get { return _payment.Amount.TenderType; } }
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get { return new Money(_payment.Amount.Amount); } }
    }
}
