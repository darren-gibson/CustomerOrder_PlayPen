namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    using System;
    using System.Xml.Serialization;
    using Model;

    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914")]
    [Serializable]
    public class OrderPricedEvent :  ICustomerOrderBasedEvent
    {
        public OrderPricedEvent() { } // required for serialization
        public OrderPricedEvent(Guid eventId, ICustomerOrder customerOrder, IPricedOrder pricedOrder)
        {
            EventId = eventId.ToString();
            Order = customerOrder.Id.ToString();
            NetTotal = new SerializedPrice(pricedOrder.NetTotal);
        }

        [XmlIgnore]
        public string EventId { get; set; }
        [XmlElement(ElementName = "order")]
        public string Order { get; set; }
        [XmlElement(ElementName = "netTotal")]
        public SerializedPrice NetTotal { get; set; }
    }
}
