namespace CustomerOrder.Model
{
    public interface IPricedOrder
    {
        IProductPrice GetProductPrice(IProduct productEntryToGetPriceFor);
        Money NetTotal { get; }
    }
}
