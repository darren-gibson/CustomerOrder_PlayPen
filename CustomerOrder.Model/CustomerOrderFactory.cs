namespace CustomerOrder.Model
{
    using System.Collections.Generic;
    using Events;

    public class CustomerOrderFactory : ICustomerOrderFactory
    {
        private readonly IPrice _priceGateway;

        public CustomerOrderFactory(IPrice priceGateway)
        {
            _priceGateway = priceGateway;
        }

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier identifier)
        {
            return new CustomerOrder(identifier, _priceGateway);
        }

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, IEnumerable<IEvent> events, IPricedOrder pricedOrder)
        {
            return new CustomerOrder(orderIdentifier, _priceGateway, events, pricedOrder);
        }
    }
}
