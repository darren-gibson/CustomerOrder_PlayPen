namespace CustomerOrder.AcceptanceTests.Contract
{
    using System;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true, Namespace = "http://api.tesco.com/order/20140914")]
    [XmlRootAttribute(Namespace = "http://api.tesco.com/order/20140914", IsNullable = false)]
    [Serializable]
    public class OrderPricedEvent
    {
        [XmlElement(ElementName = "order")]
        public string Order { get; set; }

        [XmlElement(ElementName = "netTotal")]
        public Money NetTotal { get; set; }
    }
}
