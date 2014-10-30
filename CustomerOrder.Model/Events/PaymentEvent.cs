namespace CustomerOrder.Model.Events
{
    public class PaymentEvent : Event, IPayment
    {
        public PaymentEvent(Tender amount)
        {
            Amount = amount;
        }

        public Tender Amount { get; private set; }
    }
}