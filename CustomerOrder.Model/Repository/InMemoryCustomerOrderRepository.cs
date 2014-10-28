namespace CustomerOrder.Model.Repository
{
    using System.Collections.Generic;

    public class InMemoryCustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly ICustomerOrderFactory _customerOrderFactory;
        private Dictionary<OrderIdentifier, ICustomerOrder> _orderByIdDictionary;

        public InMemoryCustomerOrderRepository(ICustomerOrderFactory customerOrderFactory)
        {
            _customerOrderFactory = customerOrderFactory;
            _orderByIdDictionary = new Dictionary<OrderIdentifier, ICustomerOrder>();
        }

        public ICustomerOrder GetOrCreateOrderById(OrderIdentifier identifier)
        {
            if (!_orderByIdDictionary.ContainsKey(identifier))
                _orderByIdDictionary.Add(identifier, _customerOrderFactory.MakeCustomerOrder(identifier));
            return _orderByIdDictionary[identifier];
        }
    }
}
