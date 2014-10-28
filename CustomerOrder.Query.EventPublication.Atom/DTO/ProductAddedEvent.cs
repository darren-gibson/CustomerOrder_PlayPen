namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    using System;
    using System.Xml.Serialization;
    using Model;

    [XmlRoot(Namespace = "http://api.tesco.com/order/20140914")]
    [Serializable]
    public class ProductAddedEvent : ICustomerOrderBasedEvent
    {
        public ProductAddedEvent() { } // just for serialization 
        public ProductAddedEvent(Guid eventId, ICustomerOrder customerOrder, IProduct productAdded)
        {
            Order = customerOrder.Id.ToString();
            Product = productAdded.ProductIdentifier.ToString();
            Price = new SerializedProductPrice(customerOrder.GetProductPrice(productAdded));
            Quantity = new SerializedQuantity(productAdded.Quantity);
            EventId = eventId.ToString();
        }

        [XmlIgnore]
        public string EventId { get; set; }

        [XmlElement(ElementName = "order")]
        public string Order { get; set; }

        [XmlElement(ElementName = "product")]
        public string Product  { get; set; }

        [XmlElement(ElementName = "quantity")]
        public SerializedQuantity Quantity { get; set; }
        
        [XmlElement(ElementName = "price")]
        public SerializedProductPrice Price { get; set; }
    }
}
