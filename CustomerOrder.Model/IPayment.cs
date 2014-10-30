namespace CustomerOrder.Model
{
    public interface IPayment
    {
        Tender Amount { get; }
    }
}
