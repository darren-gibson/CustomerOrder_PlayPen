namespace CustomerOrder.Contract.DTO
{
    using Newtonsoft.Json;

    public class ProductAdded
    {
        private readonly Model.ProductAdded _product;

        public ProductAdded(Model.ProductAdded productAdded)
        {
            _product = productAdded;
        }
        [JsonProperty(PropertyName = "productId")]
        internal string Id { get { return _product.ProductIdentifier.ToString(); } }

        [JsonProperty(PropertyName = "quantity")]
        internal Quantity Quantity { get { return new Quantity(_product.Quantity); } }

        [JsonProperty(PropertyName = "price")]
        internal ProductPrice Price { get { return new ProductPrice(_product.UnitPrice, _product.NetPrice); } }
    }
}