namespace CustomerOrder.Contract.DTO
{
    using Model;
    using Newtonsoft.Json;

    public class Price
    {
        private readonly Money _price;

        public Price(Money price)
        {
            _price = price;
        }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get { return decimal.Parse(_price.ToString("N", null)); } }
        [JsonProperty(PropertyName = "currency")]
        public string  Currency { get { return _price.Code.ToString(); } }
    }
}