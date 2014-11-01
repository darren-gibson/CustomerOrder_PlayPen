namespace CustomerOrder.Model
{
    using System;
    using System.Collections.Generic;

    public interface ICustomerOrder
    {
        OrderIdentifier Id { get; }
        Currency Currency { get; }
        ProductAdded ProductAdd(ProductIdentifier productIdentifier, Quantity requiredQuantity);
        PaymentAdded PaymentAdd(Tender amount);
        IEnumerable<IProduct> Products { get; }
        IEnumerable<IPayment> Payments { get; }
        IProductPrice GetProductPrice(IProduct product);
        Money NetTotal { get; }
        Money AmountDue { get; }
        Money AmountPaid { get; }
        event EventHandler<ProductAddedEventArgs> ProductAdded;
        event EventHandler<OrderPricedEventArgs> OrderPriced;
    }
}
