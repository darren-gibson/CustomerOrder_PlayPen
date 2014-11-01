namespace CustomerOrder.Contract.DTO
{
    using Model;
    using Newtonsoft.Json;

    public class OrderTotal
    {
        private readonly ICustomerOrder _order;

        public OrderTotal(ICustomerOrder order)
        {
            _order = order;
        }
        [JsonProperty(PropertyName = "net")]
        public Money Net { get { return new Money(_order.NetTotal); } }

    }
}
