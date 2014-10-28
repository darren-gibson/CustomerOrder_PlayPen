namespace CustomerOrder.Query.EventPublication.Atom
{
    using DTO;
    using Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Syndication;

    public class SimpleInMemoryAtomEventRepository : IAtomEventRepository
    {
        private readonly SynchronizedCollection<CustomerOrderBasedSyndicationItem> _events;

        public SimpleInMemoryAtomEventRepository()
        {
            _events = new SynchronizedCollection<CustomerOrderBasedSyndicationItem>();
        }

        public IEnumerable<CustomerOrderGeneratedEventSyndicationItem<T>> GetAllEventsInCurrentFeed<T>() where T : ICustomerOrderBasedEvent
        {
            return _events.Where(e => e is CustomerOrderGeneratedEventSyndicationItem<T>).Cast <CustomerOrderGeneratedEventSyndicationItem<T>>();
        }

        public IEnumerable<CustomerOrderBasedSyndicationItem> GetAllEventsForOrderInCurrentFeed(OrderIdentifier orderIdentifier)
        {
            return _events.Where(e => e.OrderId.Equals(orderIdentifier));
        }

        public IEnumerable<CustomerOrderGeneratedEventSyndicationItem<T>> GetAllEventsOfTypeForOrderInCurrentFeed<T>(OrderIdentifier matching) where T : ICustomerOrderBasedEvent
        {
            return GetAllEventsInCurrentFeed<T>().Where(o => o.OrderId.Equals(matching));
        }

        public void SaveEventToCurrentFeed<T>(CustomerOrderGeneratedEventSyndicationItem<T> eventToSave) where T : ICustomerOrderBasedEvent
        {
            _events.Add(eventToSave);
        }
    }
}
