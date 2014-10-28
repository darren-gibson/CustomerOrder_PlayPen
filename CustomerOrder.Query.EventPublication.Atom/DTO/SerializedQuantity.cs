namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    using System.Xml.Serialization;
    using Model;

    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914")]
    public class SerializedQuantity
    {
        public SerializedQuantity() { } // required for XML serialization
        public SerializedQuantity(Quantity quantity)
        {
            Amount = quantity.ToString("a", null);
            UnitOfMeasure = quantity.ToString("u", null);
        }

        [XmlAttribute(AttributeName = "uom")]
        public string UnitOfMeasure { get; set; }

        [XmlText]
        public string Amount { get; set; }
    }
}