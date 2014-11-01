namespace CustomerOrder.Contract.DTO
{
    using Newtonsoft.Json;

    public class PaymentAdded
    {
        private readonly Model.PaymentAdded _paymentAdded;

        public PaymentAdded(Model.PaymentAdded paymentAdded)
        {
            _paymentAdded = paymentAdded;
        }

        [JsonProperty(PropertyName = "tenderType")]
        public string TenderType { get { return _paymentAdded.Amount.TenderType; } }
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get { return new Money(_paymentAdded.Amount.Amount); } }
    }
}
