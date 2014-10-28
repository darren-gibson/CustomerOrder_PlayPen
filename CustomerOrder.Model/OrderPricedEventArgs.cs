namespace CustomerOrder.Model
{
    using System;

    public class OrderPricedEventArgs : EventArgs
    {
        public OrderPricedEventArgs(IPricedOrder pricedOrder)
        {
            PricedOrder = pricedOrder;
        }

        public IPricedOrder PricedOrder { get; private set; }
    }
}