namespace CustomerOrder.Model.Repository
{
    using System.Threading.Tasks;

    public interface ICustomerOrderRepository
    {
        Task<ICustomerOrder> GetOrCreateOrderById(OrderIdentifier identifier);
    }
}
