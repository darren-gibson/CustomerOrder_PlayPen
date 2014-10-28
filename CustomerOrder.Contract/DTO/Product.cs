namespace CustomerOrder.Contract.DTO
{
    using Model;
    using Newtonsoft.Json;

    public class Product
    {
        private readonly IProduct _product;
        private readonly ICustomerOrder _order;

        public Product(IProduct product, ICustomerOrder order)
        {
            _product = product;
            _order = order;
        }


        [JsonProperty(PropertyName = "productId")]
        public string Id { get { return _product.ProductIdentifier.ToString(); } }

        [JsonProperty(PropertyName = "quantity")]
        public Quantity Quantity { get { return new Quantity(_product.Quantity); } }

        [JsonProperty(PropertyName = "price")]
        public ProductPrice Price { get { return new ProductPrice(_order.GetProductPrice(_product)); } }
    }
}