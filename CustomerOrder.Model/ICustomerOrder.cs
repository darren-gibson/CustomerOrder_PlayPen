namespace CustomerOrder.Model
{
    using System;
    using System.Collections.Generic;

    public interface ICustomerOrder
    {
        OrderIdentifier Id { get; }

        ProductAdded ProductAdd(ProductIdentifier productIdentifier, Quantity requiredQuantity);
        IEnumerable<IProduct> Products { get; }
        IProductPrice GetProductPrice(IProduct product);
        Money NetTotal { get; }
        event EventHandler<ProductAddedEventArgs> ProductAdded;
        event EventHandler<OrderPricedEventArgs> OrderPriced;
    }
}
