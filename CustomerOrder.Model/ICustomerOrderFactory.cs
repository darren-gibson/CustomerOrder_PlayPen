namespace CustomerOrder.Model
{
    using System.Collections.Generic;
    using Events;

    public interface ICustomerOrderFactory
    {
        ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, Currency currency);
        ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, Currency currency, IEnumerable<IEvent> events, IPricedOrder pricedOrder);
    }
}
