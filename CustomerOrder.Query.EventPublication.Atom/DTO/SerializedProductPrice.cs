namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    using System.Xml.Serialization;
    using Model;
    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914")]
    public class SerializedProductPrice
    {
        public SerializedProductPrice() { } // required for deserialization

        public SerializedProductPrice(IProductPrice productPrice)
        {
            UnitPrice = new SerializedPrice(productPrice.UnitPrice);
            NetPrice = new SerializedPrice(productPrice.NetPrice);
        }
        [XmlElement(ElementName = "unit")]
        public SerializedPrice UnitPrice { get; set; }
        
        [XmlElement(ElementName = "net")]
        public SerializedPrice NetPrice { get; set; }
    }
}
