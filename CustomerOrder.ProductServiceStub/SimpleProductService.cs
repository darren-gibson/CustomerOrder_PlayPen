namespace CustomerOrder.ProductServiceStub
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    public class SimpleProductService
    {
        private readonly Dictionary<string, Product> _products;

        public SimpleProductService()
        {
            _products = new Dictionary<string, Product>();
        }
        public void AddProduct(string productId, string description, string imageUrl)
        {
            var product = new Product(productId, description, imageUrl);
            _products.Add(productId, product);
        }

        public void Serialize(IEnumerable<string> products, Stream stream)
        {
            var result = new SearchResult();
            foreach (var productId in products)
            {
                if (_products.ContainsKey(productId))
                    result.Products.Add(_products[productId]);
                else   
                    result.MissingSet.Add(productId);
            }

            SerializeResult(result, stream);
        }

        private void SerializeResult(SearchResult result, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonTextWriter, result);
                }
            }
        }
    }
}
