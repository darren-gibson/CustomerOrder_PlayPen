namespace CustomerOrder.PriceServiceStub
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class SimplePriceGateway : IPrice
    {
        private readonly Dictionary<ProductIdentifier, QuantityPrice> _priceLookup;

        public SimplePriceGateway()
        {
            _priceLookup = new Dictionary<ProductIdentifier, QuantityPrice>
            {
                { "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b", new QuantityPrice(new Money(Currency.GBP, 0.50m), 1) }
            };
        }

        public void SetPrice(ProductIdentifier productIdentifier, QuantityPrice price)
        {
            _priceLookup[productIdentifier] = price;
        }

        public IPricedOrder Price(ICustomerOrder order)
        {
            var productPrices = order.Products.ToDictionary(product => product, PriceProduct);
            return new PricedOrder(productPrices);
        }

        private PricedProduct PriceProduct(IProduct product)
        {
            var quantityPrice = GetQuantityPrice(product);
            var unitPrice = quantityPrice * new Quantity(1, UnitOfMeasure.Each);
            var netPrice = quantityPrice * product.Quantity;

            return new PricedProduct(product, unitPrice, netPrice);
        }

        private QuantityPrice GetQuantityPrice(IProduct product)
        {
            if(!_priceLookup.ContainsKey(product.ProductIdentifier))
                throw new ProductNotFoundException(product.ProductIdentifier);
            return _priceLookup[product.ProductIdentifier];
        }
    }
}
