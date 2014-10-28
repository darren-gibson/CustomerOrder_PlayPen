namespace CustomerOrder.Model.Events
{
    using System;

    public class ProductAddedEvent : Event, IProduct
    {
        public ProductAddedEvent(ProductIdentifier productIdentifier, Quantity quantity)
        {
            ProductIdentifier = productIdentifier;
            Quantity = quantity;
        }

        public ProductIdentifier ProductIdentifier { get; private set; }
        public Quantity Quantity { get; private set; }
    }
}