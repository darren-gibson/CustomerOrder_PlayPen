namespace CustomerOrder.Model.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryCustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly ICustomerOrderFactory _customerOrderFactory;
        private readonly Currency _defaultCurrency;
        private readonly Dictionary<OrderIdentifier, ICustomerOrder> _orderByIdDictionary;

        public InMemoryCustomerOrderRepository(ICustomerOrderFactory customerOrderFactory, Currency defaultCurrency)
        {
            _customerOrderFactory = customerOrderFactory;
            _defaultCurrency = defaultCurrency;
            _orderByIdDictionary = new Dictionary<OrderIdentifier, ICustomerOrder>();
        }

        public Task<ICustomerOrder> GetOrCreateOrderById(OrderIdentifier identifier)
        {
            return Task.Factory.StartNew(() => InternalGetOrCreateOrderById(identifier));
        }

        private ICustomerOrder InternalGetOrCreateOrderById(OrderIdentifier identifier)
        {
            if (!_orderByIdDictionary.ContainsKey(identifier))
                _orderByIdDictionary.Add(identifier, _customerOrderFactory.MakeCustomerOrder(identifier, _defaultCurrency));
            return _orderByIdDictionary[identifier];
        }
    }
}
