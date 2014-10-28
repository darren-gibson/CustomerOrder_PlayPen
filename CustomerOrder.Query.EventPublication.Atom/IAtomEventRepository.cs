namespace CustomerOrder.Query.EventPublication.Atom
{
    using System.Collections.Generic;
    using System.ServiceModel.Syndication;
    using DTO;
    using Model;

    public interface IAtomEventRepository
    {
        void SaveEventToCurrentFeed<T>(CustomerOrderGeneratedEventSyndicationItem<T> eventToSave) where T : ICustomerOrderBasedEvent;
        IEnumerable<CustomerOrderGeneratedEventSyndicationItem<T>> GetAllEventsInCurrentFeed<T>() where T : ICustomerOrderBasedEvent;
        IEnumerable<CustomerOrderGeneratedEventSyndicationItem<T>> GetAllEventsOfTypeForOrderInCurrentFeed<T>(OrderIdentifier matching) where T : ICustomerOrderBasedEvent;
        IEnumerable<CustomerOrderBasedSyndicationItem> GetAllEventsForOrderInCurrentFeed(OrderIdentifier orderIdentifier);
        
    }
}
