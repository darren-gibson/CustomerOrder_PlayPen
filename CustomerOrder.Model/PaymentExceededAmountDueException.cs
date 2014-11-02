namespace CustomerOrder.Model
{
    using System;

    public class PaymentExceededAmountDueException : Exception
    {
        public PaymentExceededAmountDueException(Money paymentAmount, Money amountDue) :
            base(string.Format("Payment of {0} exceeded amount due of {1}", paymentAmount, amountDue))
        {
            AmountDue = amountDue;
        }

        public Money AmountDue { get; private set; }
    }
}
