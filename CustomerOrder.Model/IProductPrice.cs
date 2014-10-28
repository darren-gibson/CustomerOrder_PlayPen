namespace CustomerOrder.Model
{
    public interface IProductPrice
    {
        Money UnitPrice { get; }
        Money NetPrice { get; }
    }
}
