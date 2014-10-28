namespace CustomerOrder.Query.EventPublication.Atom
{
    using System.ServiceModel.Syndication;
    using Model;

    public abstract class CustomerOrderBasedSyndicationItem : SyndicationItem
    {
        protected CustomerOrderBasedSyndicationItem() { }

        protected CustomerOrderBasedSyndicationItem(ICustomerOrder customerOrder)
        {
            OrderId = customerOrder.Id;
        }
        public OrderIdentifier OrderId { get; set; }
    }
}
