namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    using System.Xml.Serialization;
    using Model;

    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914")]
    public class SerializedPrice
    {
        public SerializedPrice() { } // required for XML serialization

        public SerializedPrice(Money price)
        {
            Value = string.Format("{0:n}", price);
            Currency = price.Code.ToString();
        }

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}