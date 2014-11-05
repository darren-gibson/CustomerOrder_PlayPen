namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Model;

    class CustomerOrderBaseDouble : ICustomerOrder
    {
        public OrderIdentifier Id { get; private set; }
        public Currency Currency { get; private set; }


        public virtual ProductAdded ProductAdd(ProductIdentifier productIdentifier, Quantity requiredQuantity)
        {
            throw new NotImplementedException();
        }

        public PaymentAdded PaymentAdd(Tender amount)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IProduct> Products
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IPayment> Payments
        {
            get { throw new NotImplementedException(); }
        }

        public Money AmountDue { get { throw new NotImplementedException(); } }
        public Money AmountPaid { get; private set; }
        public CustomerOrderStatus Status { get; private set; }
        public event EventHandler<ProductAddedEventArgs> ProductAdded;
        public event EventHandler<OrderPricedEventArgs> OrderPriced;


        public virtual IProductPrice GetProductPrice(IProduct product)
        {
            throw new NotImplementedException();
        }

        public Money NetTotal { get; private set; }

        public bool IsProductAddedSubscribed { get { return ProductAdded != null; } }
        public bool IsOrderPricedSubscribed { get { return OrderPriced != null; } }

        protected void OnProductAdded(ProductAddedEventArgs e)
        {
            EventHandler<ProductAddedEventArgs> handler = ProductAdded;
            if (handler != null) handler(this, e);
        }
        protected void OnOrderPriced(OrderPricedEventArgs e)
        {
            EventHandler<OrderPricedEventArgs> handler = OrderPriced;
            if (handler != null) handler(this, e);
        }
    }
}
