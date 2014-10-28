namespace CustomerOrder.Host.Query.Events
{
    using CustomerOrder.Query.EventPublication.Atom;

    public class AllEventsForOrderSyndicationFeedResponse : AbstractSyndicationFeedResponse
    {
        public AllEventsForOrderSyndicationFeedResponse(string orderId, IAtomEventRepository repository)
            : base(repository.GetAllEventsForOrderInCurrentFeed(orderId))
        {
        }
    }
}