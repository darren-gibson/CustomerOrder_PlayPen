namespace CustomerOrder.Host.Query.Events
{
    using System.Collections.Generic;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using CustomerOrder.Query.EventPublication.Atom;
    using Nancy;

    public abstract class AbstractSyndicationFeedResponse : Response
    {
        protected AbstractSyndicationFeedResponse(IEnumerable<CustomerOrderBasedSyndicationItem> items)
        {
            ContentType = "application/atom+xml";
            var feed = new SyndicationFeed(items);

            Contents = s =>
            {
                using (var atomWriter = XmlWriter.Create(s))
                {
                    var formatter = new Atom10FeedFormatter(feed);
                    formatter.WriteTo(atomWriter);
                }
            };
        }
    }
}
