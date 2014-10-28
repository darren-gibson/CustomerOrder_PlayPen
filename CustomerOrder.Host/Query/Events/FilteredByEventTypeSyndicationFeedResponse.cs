namespace CustomerOrder.Host.Query.Events
{
    using CustomerOrder.Query.EventPublication.Atom;
    using CustomerOrder.Query.EventPublication.Atom.DTO;

    public class FilteredByEventTypeSyndicationFeedResponse<T> : AbstractSyndicationFeedResponse where T : ICustomerOrderBasedEvent
    {
        public FilteredByEventTypeSyndicationFeedResponse(string orderId, IAtomEventRepository repository) : base(repository.GetAllEventsOfTypeForOrderInCurrentFeed<T>(orderId))
        {
        }
    }
}