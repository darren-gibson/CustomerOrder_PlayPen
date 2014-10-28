namespace CustomerOrder.Model
{
    using System;

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(ProductIdentifier productIdentifier) : base(string.Format("{0}: was not found", productIdentifier))
        {
        }
    }
}
