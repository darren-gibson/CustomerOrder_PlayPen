namespace CustomerOrder.Model.Order
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

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier identifier, Currency currency)
        {
            return new CustomerOrder(identifier, currency, _priceGateway);
        }

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, Currency currency, IEnumerable<IEvent> events, IPricedOrder pricedOrder)
        {
            return new CustomerOrder(orderIdentifier, currency, _priceGateway, events, pricedOrder);
        }
    }
}
