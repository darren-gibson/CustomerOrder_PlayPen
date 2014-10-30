namespace CustomerOrder.Model
{
    public class PaymentAdded
    {
        public PaymentAdded(Tender amount)
        {
            Amount = amount;
        }

        public Tender Amount { get; private set; }
    }
}