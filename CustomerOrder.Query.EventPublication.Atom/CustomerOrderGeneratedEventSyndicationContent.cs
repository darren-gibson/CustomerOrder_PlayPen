namespace CustomerOrder.Query.EventPublication.Atom
{
    using System.ServiceModel.Syndication;
    using System.Xml;
    using System.Xml.Serialization;
    using DTO;

    public class CustomerOrderGeneratedEventSyndicationContent<T> : SyndicationContent where T : ICustomerOrderBasedEvent
    {
        private readonly T _eventDTO;

        public CustomerOrderGeneratedEventSyndicationContent(T eventDTO)
        {
            _eventDTO = eventDTO;
        }

        public override SyndicationContent Clone()
        {
            return new CustomerOrderGeneratedEventSyndicationContent<T>(_eventDTO);
        }

        public override string Type
        {
            get
            {
                return string.Format("application/vnd.tesco.{0}+xml", _eventDTO.GetType().Name);
            }
        }

        protected override void WriteContentsTo(XmlWriter writer)
        {
            var ser = new XmlSerializer(typeof(T));
            ser.Serialize(writer, _eventDTO);
        }
    }
}
