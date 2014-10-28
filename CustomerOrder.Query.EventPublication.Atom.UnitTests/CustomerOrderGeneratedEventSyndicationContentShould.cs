namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Atom.DTO;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerOrderGeneratedEventSyndicationContentShould
    {
        private CustomerOrderGeneratedEventSyndicationContent<DummyEventDouble> _contentUnderTest;

        [SetUp]
        public void SetUp()
        {
            _contentUnderTest = new CustomerOrderGeneratedEventSyndicationContent<DummyEventDouble>(new DummyEventDouble());
        }

        [Test]
        public void SetTheTypeBasedOnTheTypePassedIn()
        {
            Assert.AreEqual("application/vnd.tesco.DummyEventDouble+xml", _contentUnderTest.Type);
        }

        [Test]
        public void SerializeTheContentToXml()
        {
            string xmlWritten;

            var memStm = new MemoryStream();
            using (var writer = new XmlTextWriter(memStm, Encoding.UTF8))
            {
                _contentUnderTest.WriteTo(writer, "Content", "http://test.org");
                writer.Flush();

                memStm.Seek(0, SeekOrigin.Begin);
                var streamReader = new StreamReader(memStm);
                xmlWritten = streamReader.ReadToEnd();
            }

            const string expectedXml =
                "<Content type=\"application/vnd.tesco.DummyEventDouble+xml\" xmlns=\"http://test.org\">" +
                    "<DummyEventDouble>" +
                        "<Name>Hello</Name>" +
                    "</DummyEventDouble>" +
                "</Content>";

            xmlWritten = XmlHelper.RemoveDefaultSchema(xmlWritten);
            Assert.AreEqual(expectedXml, xmlWritten);
        }

        [Test]
        public void CreateAClone()
        {
            var clone = _contentUnderTest.Clone();
            Assert.AreEqual("application/vnd.tesco.DummyEventDouble+xml", clone.Type);
        }

        [XmlRoot("DummyEventDouble", Namespace = "http://test.org", IsNullable = false)]
        public class DummyEventDouble : ICustomerOrderBasedEvent
        {
            public DummyEventDouble()
            {
                EventId = Guid.NewGuid().ToString();
            }
            public string Name
            {
                get { return "Hello"; }
                set { if (value == null) throw new ArgumentNullException("value"); }
            }

            [XmlIgnore]
            public string EventId { get; set; }
            public string Order { get; set; }
        }
    }
}

