namespace CustomerOrder.Model
{
    using System;
    using System.Collections.Generic;
    using Events;

    public class EventRasingCustomerOrderFactory : ICustomerOrderFactory
    {
        private readonly ICustomerOrderFactory _customerOrderFactory;

        public EventRasingCustomerOrderFactory(ICustomerOrderFactory customerOrderFactory)
        {
            _customerOrderFactory = customerOrderFactory;
        }

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier)
        {
            var customerOrder = _customerOrderFactory.MakeCustomerOrder(orderIdentifier);
            OnCustomerOrderMade(new CustomerOrderMadeEventArgs(customerOrder));
            return customerOrder;
        }

        public ICustomerOrder MakeCustomerOrder(OrderIdentifier orderIdentifier, IEnumerable<IEvent> events, IPricedOrder pricedOrder)
        {
            var customerOrder = _customerOrderFactory.MakeCustomerOrder(orderIdentifier, events, pricedOrder);
            OnCustomerOrderMade(new CustomerOrderMadeEventArgs(customerOrder));
            return customerOrder;
        }

        public event EventHandler<CustomerOrderMadeEventArgs> CustomerOrderMade;

        protected virtual void OnCustomerOrderMade(CustomerOrderMadeEventArgs e)
        {
            EventHandler<CustomerOrderMadeEventArgs> handler = CustomerOrderMade;
            if (handler != null) handler(this, e);
        }
    }
}
