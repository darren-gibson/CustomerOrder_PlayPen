namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    using System;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Atom.DTO;
    using Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderGeneratedEventSyndicationItemShould
    {
        private CustomerOrderGeneratedEventSyndicationItem<DummSyndicationContentDouble> _itemUnderTest;
        private OrderIdentifier _expectedOrderIdentifier;
        private DummSyndicationContentDouble _expectedDto;

        [SetUp]
        public void SetUp()
        {
            _expectedDto = new DummSyndicationContentDouble();
            _expectedOrderIdentifier = Guid.NewGuid();
            var customerOderMock = new Mock<ICustomerOrder>();
            customerOderMock.SetupGet(o => o.Id).Returns(_expectedOrderIdentifier);;

            _itemUnderTest = new CustomerOrderGeneratedEventSyndicationItem<DummSyndicationContentDouble>(customerOderMock.Object, _expectedDto);            
        }


        [Test]
        public void SetTheTitleBasedOnTheTypePassedInTheConstructor()
        {
            Assert.AreEqual("DummSyndicationContentDouble", _itemUnderTest.Title.Text);
            Assert.AreEqual("text", _itemUnderTest.Title.Type);
        }

        [Test]
        public void SetTheIdToTheEventIdPassedInTheEvent()
        {
            Assert.AreEqual(_expectedDto.EventId, _itemUnderTest.Id);
        }

        [Test]
        public void SetTheContentToTheContentForTheDTOPassedInOnTheConstructor()
        {
            Assert.IsInstanceOf(typeof(CustomerOrderGeneratedEventSyndicationContent<DummSyndicationContentDouble>), _itemUnderTest.Content);
        }

        [Test]
        public void SetTheOrderIdentifierToThatPassedInTheConstructor()
        {
            Assert.AreEqual(_expectedOrderIdentifier, _itemUnderTest.OrderId);
        }

        private class DummSyndicationContentDouble : SyndicationContent, ICustomerOrderBasedEvent
        {
            public DummSyndicationContentDouble()
            {
                EventId = Guid.NewGuid().ToString();
            }
            public override SyndicationContent Clone()
            {
                throw new NotImplementedException();
            }

            protected override void WriteContentsTo(XmlWriter writer)
            {
                throw new NotImplementedException();
            }

            public override string Type
            {
                get { return "DummSyndicationContentDouble"; }
            }

            public string EventId { get; set; }
            public string Order { get; set; }
        }
    }
}

