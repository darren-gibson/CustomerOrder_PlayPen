using NUnit.Framework;

namespace CustomerOrder.Query.EventPublication.Atom.UnitTests.DTO
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using Atom.DTO;

    [TestFixture]
    public abstract class ICustomerOrderBasedEventShould<T> where T : ICustomerOrderBasedEvent
    {
        protected T EventUnderTest;

        [Test]
        public void ImplementsICustomerOrderBasedEvent()
        {
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.IsTrue(EventUnderTest is ICustomerOrderBasedEvent);
        }

        [Test]
        public void SerializeToXmlCorrectly()
        {
            var expectedXml = GetExpectedXml();
            var actualXml = XmlSerializeSerializeObject(EventUnderTest);
            actualXml = XmlHelper.RemoveDefaultSchema(actualXml);

            Assert.AreEqual(expectedXml, actualXml);
        }

        protected abstract string GetExpectedXml();

        private static string XmlSerializeSerializeObject(T objectToSerialize)
        {
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var stringWriter = new StringWriter())
            {
                var writer = XmlWriter.Create(stringWriter, settings);
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                var serializer = new XmlSerializer(typeof(T));

                serializer.Serialize(writer, objectToSerialize);
                return stringWriter.ToString();
            }
        }
    }
}
