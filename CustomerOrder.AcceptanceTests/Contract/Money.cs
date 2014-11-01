namespace CustomerOrder.AcceptanceTests.Contract
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(AnonymousType = true, Namespace = "http://api.tesco.com/order/20140914")]
    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914", IsNullable = false)]
    [Serializable]
    public class Money
    {
        [XmlText]
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        [XmlAttribute(AttributeName = "currency")]
        [JsonProperty(PropertyName = "currency")]
        public string CurrencyCode { get; set; }
    }
}