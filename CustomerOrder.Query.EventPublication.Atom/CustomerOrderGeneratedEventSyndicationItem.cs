namespace CustomerOrder.Query.EventPublication.Atom
{
    using System.ServiceModel.Syndication;
    using DTO;
    using Model;

    public class CustomerOrderGeneratedEventSyndicationItem<T> : CustomerOrderBasedSyndicationItem where T : ICustomerOrderBasedEvent
    {
        public CustomerOrderGeneratedEventSyndicationItem() { }
        public CustomerOrderGeneratedEventSyndicationItem(ICustomerOrder customerOrder, T content) : base(customerOrder)
        {
            Title = new TextSyndicationContent(typeof(T).Name);
            Content = new CustomerOrderGeneratedEventSyndicationContent<T>(content);
            Id = content.EventId;
        }
    }
}
