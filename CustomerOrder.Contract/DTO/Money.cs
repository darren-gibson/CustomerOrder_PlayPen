namespace CustomerOrder.Contract.DTO
{
    using Annotations;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Money
    {
        public Money() { }
        public Money(Model.Money money)
        {
            Amount = decimal.Parse(money.ToString("N", null));
            Currency = money.Code;
        }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "currency"), JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }

        public Model.Money ToMoney()
        {
            return new Model.Money(Currency, Amount);
        }
    }
}