namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    using System;
    using System.ServiceModel.Syndication;
    using Atom.DTO;
    using Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleInMemoryAtomEventRepositoryShould
    {
        private IAtomEventRepository _repositoryUnderTest;

        [SetUp]
        public void SetUp()
        {
            _repositoryUnderTest = new SimpleInMemoryAtomEventRepository();
        }

        [Test]
        public void ReturnAnEventThatIsAddedToTheRepository()
        {
            var expectedEvent = new EventSyndicationItem();
            _repositoryUnderTest.SaveEventToCurrentFeed(expectedEvent);

            var events = _repositoryUnderTest.GetAllEventsInCurrentFeed<ICustomerOrderBasedEvent>();

            CollectionAssert.Contains(events, expectedEvent);
        }

        [Test]
        public void ReturnEventsInTheOrderTheyWereAdded()
        {
            var expectedEvents = new[] { new EventSyndicationItem(), new EventSyndicationItem(), new EventSyndicationItem() };

            foreach (var expectedEvent in expectedEvents)
                _repositoryUnderTest.SaveEventToCurrentFeed(expectedEvent);

            var events = _repositoryUnderTest.GetAllEventsInCurrentFeed<ICustomerOrderBasedEvent>();

            CollectionAssert.AreEqual(expectedEvents, events);
        }

        [Test]
        public void ReturnOnlyTheEventsThatMatchTheOrderSpecified()
        {
            OrderIdentifier matching = Guid.NewGuid();
            OrderIdentifier notMatching = Guid.NewGuid();
            var matchingEvent1 = new EventSyndicationItem(matching);
            var notMatchingEvent2 = new EventSyndicationItem(notMatching);
            var matchingEvent3 = new EventSyndicationItem(matching);

            _repositoryUnderTest.SaveEventToCurrentFeed(matchingEvent1);
            _repositoryUnderTest.SaveEventToCurrentFeed(notMatchingEvent2);
            _repositoryUnderTest.SaveEventToCurrentFeed(matchingEvent3);

            var events = _repositoryUnderTest.GetAllEventsOfTypeForOrderInCurrentFeed<ICustomerOrderBasedEvent>(matching);

            CollectionAssert.AreEqual(new[] { matchingEvent1, matchingEvent3 }, events);
        }

        [Test]
        public void ReturnAllEventsForAnOrderRegardlessOfTheEventType()
        {
            OrderIdentifier orderIdentifier = Guid.NewGuid();
            var event1 = new EventSyndicationItem(orderIdentifier);
            var event2 = new CustomerOrderGeneratedEventSyndicationItem<CustomerOrderBasedEventDouble>() { OrderId = orderIdentifier.ToString() };

            _repositoryUnderTest.SaveEventToCurrentFeed(event1);
            _repositoryUnderTest.SaveEventToCurrentFeed(event2);

            var events = _repositoryUnderTest.GetAllEventsForOrderInCurrentFeed(orderIdentifier);

            CollectionAssert.AreEqual(new SyndicationItem[] { event1, event2 }, events);
        }

        private class EventSyndicationItem : CustomerOrderGeneratedEventSyndicationItem<ICustomerOrderBasedEvent>
        {
            public EventSyndicationItem() { }

            public EventSyndicationItem(OrderIdentifier orderIdentifier)
            {
                OrderId = orderIdentifier.ToString();
            }
        }

        private class CustomerOrderBasedEventDouble : ICustomerOrderBasedEvent
        {
            public string EventId { get; set; }
            public string Order { get; set; }
        }
    }
}