namespace CustomerOrder.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Events;

    public sealed class CustomerOrder : ICustomerOrder
    {
        private readonly IPrice _priceGateway;
        private readonly List<IEvent> _events;
        private IPricedOrder _pricedOrder;
        internal CustomerOrder(OrderIdentifier identifier, IPrice priceGateway)
            : this(identifier, priceGateway, new IEvent[] { }, null) { }

        // ReSharper disable once TooManyDependencies : This is an aggregate root, hence a lot of dependencies.
        internal CustomerOrder(OrderIdentifier orderIdentifier, IPrice priceGateway, IEnumerable<IEvent> events, IPricedOrder pricedOrder)
        {
            _priceGateway = priceGateway;
            Id = orderIdentifier;
            _pricedOrder = pricedOrder;
            _events = new List<IEvent>(events);
            if (_pricedOrder == null)
                RepriceOrder();
        }

        public OrderIdentifier Id { get; private set; }

        public ProductAdded ProductAdd(ProductIdentifier productIdentifier, Quantity requiredQuantity)
        {
            var productAdded = CreateProductAddedEvent(productIdentifier, requiredQuantity);
            var pricedOrder = RepriceOrderAndGetProductPrice(productAdded);
            OnProductAdded(new ProductAddedEventArgs(productAdded, pricedOrder));
            return new ProductAdded(productAdded, GetProductPrice(productAdded));  
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
            var pricedOrder = RepriceOrder();
            return pricedOrder.GetProductPrice(product);
        }

        private IPricedOrder RepriceOrder()
        {
            _pricedOrder = _priceGateway.Price(this);
            OnOrderPriced(new OrderPricedEventArgs(_pricedOrder));
            return _pricedOrder;
        }

        private ProductAddedEvent CreateProductAddedEvent(ProductIdentifier productIdentifier, Quantity requiredQuantity)
        {
            var productAdded = new ProductAddedEvent(productIdentifier, requiredQuantity);
            _events.Add(productAdded);
            return productAdded;
        }

        public IEnumerable<IProduct> Products
        {
            get { return _events.Where(e => e is IProduct).Cast<IProduct>(); }
        }

        public event EventHandler<ProductAddedEventArgs> ProductAdded;
        public event EventHandler<OrderPricedEventArgs> OrderPriced;

        public IProductPrice GetProductPrice(IProduct product)
        {
            return _pricedOrder.GetProductPrice(product);
        }

        public Money NetTotal { get { return _pricedOrder.NetTotal; } }

        private void OnProductAdded(ProductAddedEventArgs e)
        {
            EventHandler<ProductAddedEventArgs> handler = ProductAdded;
            if (handler != null) handler(this, e);
        }
        private void OnOrderPriced(OrderPricedEventArgs e)
        {
            EventHandler<OrderPricedEventArgs> handler = OrderPriced;
            if (handler != null) handler(this, e);
        }
    }
}