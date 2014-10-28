namespace CustomerOrder.Model
{
    using System;

    public class CustomerOrderMadeEventArgs : EventArgs
    {
        public ICustomerOrder CustomerOrder { get; private set; }

        public CustomerOrderMadeEventArgs(ICustomerOrder customerOrder)
        {
            CustomerOrder = customerOrder;
        }
    }
}