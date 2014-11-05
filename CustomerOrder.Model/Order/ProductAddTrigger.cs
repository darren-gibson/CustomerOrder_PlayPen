namespace CustomerOrder.Model.Order
{
    using System;
    using System.Collections.Generic;
    using Events;

    internal class ProductAddTrigger : ITrigger<ProductAdded>
    {
        private readonly List<IEvent> _events;
        private readonly ProductIdentifier _productIdentifier;
        private readonly Quantity _requiredQuantity;
        private readonly RepriceOrderDelegate _repriceOrder;
        private readonly OnProductAddedDelegate _onProductAdded;

        internal delegate IPricedOrder RepriceOrderDelegate();
        internal delegate void OnProductAddedDelegate(ProductAddedEventArgs e);

        public ProductAddTrigger(List<IEvent> events, ProductIdentifier productIdentifier, Quantity requiredQuantity, RepriceOrderDelegate repriceOrder, OnProductAddedDelegate onProductAdded)
        {
            _events = events;
            _productIdentifier = productIdentifier;
            _requiredQuantity = requiredQuantity;
            _repriceOrder = repriceOrder;
            _onProductAdded = onProductAdded;
        }

        public Trigger TriggerType { get { return Trigger.ProductAdd; } }

        public ProductAdded Execute()
        {
            // TODO: Need to ensure not set if operation fails (need a Test to Fail)
            var productAdded = CreateProductAddedEvent(_productIdentifier, _requiredQuantity);
            var productPrice = RepriceOrderAndGetProductPrice(productAdded);
            _onProductAdded(new ProductAddedEventArgs(productAdded, productPrice));
            return new ProductAdded(productAdded, productPrice);
        }

        private ProductAddedEvent CreateProductAddedEvent(ProductIdentifier productIdentifier, Quantity requiredQuantity)
        {
            var productAdded = new ProductAddedEvent(productIdentifier, requiredQuantity);
            _events.Add(productAdded);
            return productAdded;
        }

        private IProductPrice RepriceOrderAndGetProductPrice(ProductAddedEvent productAdded)
        {
            try
            {
                return RepriceOrderAndGetProductUnitPrice(productAdded);
            }
            catch (Exception)
            {
                RemoveEvent(productAdded);
                throw;
            }
        }

        private void RemoveEvent(IEvent eventToRemove)
        {
            _events.Remove(eventToRemove);
        }

        private IProductPrice RepriceOrderAndGetProductUnitPrice(IProduct product)
        {
            var pricedOrder = _repriceOrder();
            return pricedOrder.GetProductPrice(product);
        }
    }
}
