namespace CustomerOrder.Model.Command
{
    using System;
    using Repository;

    public class PaymentAddCommand : AbstractCommand
    {
        private readonly ICustomerOrderRepository _repository;
        private readonly OrderIdentifier _orderIdentifier;
        private readonly Tender _amount;

        public PaymentAddCommand(ICustomerOrderRepository repository, OrderIdentifier orderIdentifier, Tender amount, Guid? id = null) : base(id)
        {
            _repository = repository;
            _orderIdentifier = orderIdentifier;
            _amount = amount;
        }

        public override object Execute()
        {
            var order = _repository.GetOrCreateOrderById(_orderIdentifier);
            return order.PaymentAdd(_amount);
        }
    }
}
