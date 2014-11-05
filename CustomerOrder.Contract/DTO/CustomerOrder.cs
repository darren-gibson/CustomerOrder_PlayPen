namespace CustomerOrder.Contract.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class CustomerOrder
    {
        private readonly ICustomerOrder _order;

        public CustomerOrder(ICustomerOrder order)
        {
            _order = order;
        }

        [JsonProperty(PropertyName = "products")]
        public IEnumerable<Product> Products { get { return _order.Products.Select(p => new Product(p, _order)); } }
        [JsonProperty(PropertyName = "payments")]
        public IEnumerable<Payment> Payments { get { return _order.Payments.Select(p => new Payment(p)); } }
        [JsonProperty(PropertyName = "total")]
        public OrderTotal Total { get { return new OrderTotal(_order); } }
        [JsonProperty(PropertyName = "status"), JsonConverter(typeof (StringEnumConverter))]
        public CustomerOrderStatus Status { get { return _order.Status; } }
    }
}
