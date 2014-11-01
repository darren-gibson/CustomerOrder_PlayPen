namespace CustomerOrder.Contract.DTO
{
    using System;
    using Annotations;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Quantity
    {
        public Quantity() { }
        public Quantity(Model.Quantity quantity)
        {
            Amount = Decimal.Parse(quantity.ToString("a", null));
            UOM = (UnitOfMeasure)Enum.Parse(typeof(UnitOfMeasure), quantity.ToString("u", null));
        }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "uom", ItemConverterType = typeof(StringEnumConverter))]
        // ReSharper disable once InconsistentNaming
        public UnitOfMeasure UOM { get; set; }

        public Model.Quantity ToQuantity()
        {
            return new Model.Quantity(Amount, UOM);
        }
    }
}