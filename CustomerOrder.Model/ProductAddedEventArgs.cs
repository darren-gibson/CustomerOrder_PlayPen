namespace CustomerOrder.Model
{
    using System;
    using Events;

    public class ProductAddedEventArgs : EventArgs
    {
        public ProductAddedEventArgs(ProductAddedEvent productAdded, IProductPrice price)
        {
            ProductAdded = productAdded;
            Price = price;
            Id = productAdded.EventId;
        }

        public IProduct ProductAdded { get; private set; }
        public IProductPrice Price { get; private set; }
        public Guid Id { get; private set; }
    }
}