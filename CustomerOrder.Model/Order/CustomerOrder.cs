namespace CustomerOrder.Model.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Events;

    public sealed class CustomerOrder : ICustomerOrder
    {
        private readonly IPrice _priceGateway;
        private readonly List<IEvent> _events;
        private readonly CustomerOrderStateMachine _orderStateMachine;
        private IPricedOrder _pricedOrder;

        internal CustomerOrder(OrderIdentifier identifier, Currency currency, IPrice priceGateway)
            : this(identifier, currency, priceGateway, new IEvent[] { }, null) { }

        // ReSharper disable once TooManyDependencies : This is an aggregate root, hence a lot of dependencies.
        internal CustomerOrder(OrderIdentifier orderIdentifier, Currency currency, IPrice priceGateway, IEnumerable<IEvent> events, IPricedOrder pricedOrder)
        {
            _orderStateMachine = new CustomerOrderStateMachine(this);

            _priceGateway = priceGateway;
            Id = orderIdentifier;
            Currency = currency;
            _pricedOrder = pricedOrder;
            _events = new List<IEvent>(events);
            if (_pricedOrder == null)
                RepriceOrder();
        }

        public OrderIdentifier Id { get; private set; }
        public Currency Currency { get; private set; }

        public ProductAdded ProductAdd(ProductIdentifier productIdentifier, Quantity requiredQuantity)
        {
            ITrigger<ProductAdded> trigger = new ProductAddTrigger(_events, productIdentifier, requiredQuantity, RepriceOrder, OnProductAdded);
            return RunTrigger(trigger);
        }

        private T RunTrigger<T>(ITrigger<T> command)
        {
            if (_orderStateMachine.CanFire(command.TriggerType))
            {
                var result = command.Execute();
                _orderStateMachine.Fire(command.TriggerType);
                return result;
            }
            throw new InvalidOperationException(string.Format("Currently in State {0}, cannot Fire {1}", _orderStateMachine.State, command.TriggerType));
        }

        public PaymentAdded PaymentAdd(Tender amount)
        {
            var trigger = new PaymentAddTrigger(_events, amount, AmountDue);
            return RunTrigger(trigger);
        }

        private IPricedOrder RepriceOrder()
        {
            _pricedOrder = _priceGateway.Price(this);
            OnOrderPriced(new OrderPricedEventArgs(_pricedOrder));
            return _pricedOrder;
        }

        public IEnumerable<IProduct> Products
        {
            get { return _events.Where(e => e is IProduct).Cast<IProduct>(); }
        } 

        public IEnumerable<IPayment> Payments { get { return _events.Where(e => e is IPayment).Cast<IPayment>(); } }

        public Money AmountDue { get { return NetTotal - AmountPaid;} }
        public Money AmountPaid { get { return Payments.Aggregate(new Money(Currency, 0m), (current, payment) => current + payment.Amount.Amount); } }
        public CustomerOrderStatus Status { get { return _orderStateMachine.State; } }

        public event EventHandler<ProductAddedEventArgs> ProductAdded;
        public event EventHandler<OrderPricedEventArgs> OrderPriced;

        public IProductPrice GetProductPrice(IProduct product)
        {
            return _pricedOrder.GetProductPrice(product);
        }

        public Money NetTotal { get { return _pricedOrder.NetTotal; } }

        private void OnProductAdded(ProductAddedEventArgs e)
        {
            var handler = ProductAdded;
            if (handler != null) handler(this, e);
        }
        private void OnOrderPriced(OrderPricedEventArgs e)
        {
            EventHandler<OrderPricedEventArgs> handler = OrderPriced;
            if (handler != null) handler(this, e);
        }
    }
}