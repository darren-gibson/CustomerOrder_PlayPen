namespace CustomerOrder.AcceptanceTests.Contract
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(AnonymousType = true, Namespace = "http://api.tesco.com/order/20140914")]
    [XmlRootAttribute(Namespace = "http://api.tesco.com/order/20140914", IsNullable = false)]
    [Serializable]
    public class OrderPricedEvent
    {
        [XmlElement(ElementName = "order")]
        public string Order { get; set; }

        [XmlElement(ElementName = "netTotal")]
        public Price NetTotal { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://api.tesco.com/order/20140914")]
    [XmlRootAttribute(Namespace = "http://api.tesco.com/order/20140914", IsNullable = false)]
    [Serializable]
    public class Price
    {
        [XmlText]
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        [JsonProperty(PropertyName = "currency")]
        public string CurrencyCode { get; set; }
    }
}
