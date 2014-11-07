namespace CustomerOrder.Model.Order
{
    using System;
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

        public T RunTriggerAndTransitionStateIfValid<T>(ITrigger<T> command)
        {
            EnsureCanTrigger(command);
            return RunTriggerAndReturnResult(command);
        }

        private void EnsureCanTrigger<T>(ITrigger<T> command)
        {
            if (!CanFire(command.TriggerType))
                throw new InvalidOperationException(string.Format("Currently in State {0}, cannot Fire {1}", State,
                    command.TriggerType));
        }

        private T RunTriggerAndReturnResult<T>(ITrigger<T> command)
        {
            var result = command.Execute();
            Fire(command.TriggerType);
            return result;
        }
    }
}
