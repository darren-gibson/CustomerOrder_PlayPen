namespace CustomerOrder.Model.Order
{
    using Stateless;

    class CustomerOrderStateMachine : StateMachine<CustomerOrderStatus, Trigger>
    {
        private readonly CustomerOrder _order;

        public CustomerOrderStateMachine(CustomerOrder order) : base(CustomerOrderStatus.NotStarted)
        {
            _order = order;
            Configure(CustomerOrderStatus.NotStarted)
                .Permit(Trigger.ProductAdd, CustomerOrderStatus.Shopping);
            Configure(CustomerOrderStatus.Shopping).PermitReentry(Trigger.ProductAdd);
            Configure(CustomerOrderStatus.Shopping)
                .PermitIf(Trigger.PaymentAdd, CustomerOrderStatus.Complete, () => _order.AmountDue.IsZero);
            Configure(CustomerOrderStatus.Shopping)
                .PermitIf(Trigger.PaymentAdd, CustomerOrderStatus.Paying, () => !_order.AmountDue.IsZero);
            Configure(CustomerOrderStatus.Paying)
                .PermitIf(Trigger.PaymentAdd, CustomerOrderStatus.Complete, () => _order.AmountDue.IsZero);
            Configure(CustomerOrderStatus.Paying)
                .PermitReentryIf(Trigger.PaymentAdd, () => !_order.AmountDue.IsZero);            
        }
    }
}
