namespace CustomerOrder.Model
{
    public class ProductAdded
    {
        private readonly IProduct _product;
        private readonly IProductPrice _productPrice;

        public ProductAdded(IProduct product, IProductPrice productPrice)
        {
            _product = product;
            _productPrice = productPrice;
        }

        public ProductIdentifier ProductIdentifier { get { return _product.ProductIdentifier; } }
        public Quantity Quantity { get { return _product.Quantity; } }
        public Money UnitPrice { get { return _productPrice.UnitPrice; } }
        public Money NetPrice { get { return _productPrice.NetPrice; } }
    }
}
