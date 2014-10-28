namespace CustomerOrder.Host.Query
{
    using Annotations;
    using CustomerOrder.Query.EventPublication.Atom;
    using CustomerOrder.Query.EventPublication.Atom.DTO;
    using Events;
    using Nancy;

    [UsedImplicitly]
    public class AtomEventsModule : NancyModule
    {
        public AtomEventsModule(IAtomEventRepository repository)
            : base("/orders")
        {
            Get["/{orderId}/events/productAdded"] = parameters => new FilteredByEventTypeSyndicationFeedResponse<ProductAddedEvent>(parameters.orderId, repository);
            Get["/{orderId}/events/orderPriced"] = parameters => new FilteredByEventTypeSyndicationFeedResponse<OrderPricedEvent>(parameters.orderId, repository);
            Get["/{orderId}/events"] = parameters => new AllEventsForOrderSyndicationFeedResponse(parameters.orderId, repository);
        }
    }
}