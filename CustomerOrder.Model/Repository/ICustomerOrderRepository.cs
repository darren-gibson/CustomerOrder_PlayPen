namespace CustomerOrder.Model.Repository
{
    public interface ICustomerOrderRepository
    {
        ICustomerOrder GetOrCreateOrderById(OrderIdentifier identifier);
    }
}
