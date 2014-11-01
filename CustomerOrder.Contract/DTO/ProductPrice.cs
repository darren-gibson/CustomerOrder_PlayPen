namespace CustomerOrder.Contract.DTO
{
    using Model;
    using Newtonsoft.Json;

    public class ProductPrice
    {
        public ProductPrice(IProductPrice productPrice)
        {
            Unit = new Money(productPrice.UnitPrice);
            Net = new Money(productPrice.NetPrice); 
        }

        public ProductPrice(Model.Money unit, Model.Money net)
        {
            Unit = new Money(unit);
            Net =  new Money(net);
        }

        [JsonProperty(PropertyName = "unit")]
        public Money Unit { get; private set; }
        [JsonProperty(PropertyName = "net")]
        public Money Net { get; private set; }
    }
}
