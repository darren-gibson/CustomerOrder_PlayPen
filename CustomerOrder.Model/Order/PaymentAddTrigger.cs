namespace CustomerOrder.Model.Order
{
    using System.Collections.Generic;
    using Events;

    internal class PaymentAddTrigger : ITrigger<PaymentAdded>
    {
        private readonly Tender _amount;
        private readonly Money _amountDue;
        private readonly List<IEvent> _events;

        public PaymentAddTrigger(List<IEvent> events, Tender amount, Money amountDue)
        {
            _amount = amount;
            _amountDue = amountDue;
            _events = events;
        }

        public Trigger TriggerType { get { return Trigger.PaymentAdd; } }

        public PaymentAdded Execute()
        {
            EnsureCurrencyIsValid(_amount.Amount);
            if (_amount.Amount > _amountDue)
                throw new PaymentExceededAmountDueException(_amount.Amount, _amountDue);
            CreatePaymentEvent(_amount);
            return new PaymentAdded(_amount);
        }

        private void EnsureCurrencyIsValid(Money amount)
        {
            if (amount.Code != _amountDue.Code)
                throw new CurrencyDoesNotMatchOrderException(_amountDue.Code, amount.Code);
        }

        private void CreatePaymentEvent(Tender amount)
        {
            _events.Add(new PaymentEvent(amount));
        }

    }
}
