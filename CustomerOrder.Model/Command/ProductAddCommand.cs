using CustomerOrder.Model.Repository;

namespace CustomerOrder.Model.Command
{
    using System;

    public class ProductAddCommand : AbstractCommand
    {
        private readonly OrderIdentifier _orderIdentifier;
        private readonly ICustomerOrderRepository _repository;
        private readonly ProductIdentifier _productIdentifier;
        private readonly Quantity _quantity;

        public ProductAddCommand(ICustomerOrderRepository repository, OrderIdentifier orderIdentifier, ProductIdentifier productIdentifier, Quantity? quantity = null, Guid? id = null) : base(id)
        {
            _orderIdentifier = orderIdentifier;
            _repository = repository;
            _productIdentifier = productIdentifier;
            _quantity = quantity.HasValue ? quantity.Value : Quantity.Default;
        }

        public override object Execute()
        {
            var order = _repository.GetOrCreateOrderById(_orderIdentifier).Result;
            return order.ProductAdd(_productIdentifier, _quantity);
        }
    }
}
