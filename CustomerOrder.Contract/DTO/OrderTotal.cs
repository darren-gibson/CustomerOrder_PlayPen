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
        [JsonProperty(PropertyName = "due")]
        public Money Due { get { return new Money(_order.AmountDue); } }
        [JsonProperty(PropertyName = "paid")]
        public Money Paid { get { return new Money(_order.AmountPaid); } }

    }
}
