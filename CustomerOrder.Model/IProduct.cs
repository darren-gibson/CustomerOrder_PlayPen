namespace CustomerOrder.Model
{
    public interface IProduct
    {
        ProductIdentifier ProductIdentifier { get; }
        Quantity Quantity { get; }
    }
}
