namespace CustomerOrder.Model
{
    using System.Collections.Generic;
    using Events;

    public interface ICustomerOrderFactory
    {
        ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier);
        ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, IEnumerable<IEvent> events, IPricedOrder pricedOrder);
    }
}
