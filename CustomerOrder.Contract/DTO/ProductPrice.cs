namespace CustomerOrder.Contract.DTO
{
    using Model;
    using Newtonsoft.Json;

    public class ProductPrice
    {
        public ProductPrice(IProductPrice productPrice)
        {
            Unit = new Price(productPrice.UnitPrice);
            Net = new Price(productPrice.NetPrice); 
        }

        public ProductPrice(Money unit, Money net)
        {
            Unit = new Price(unit);
            Net =  new Price(net);
        }

        [JsonProperty(PropertyName = "unit")]
        public Price Unit { get; private set; }
        [JsonProperty(PropertyName = "net")]
        public Price Net { get; private set; }
    }
}
