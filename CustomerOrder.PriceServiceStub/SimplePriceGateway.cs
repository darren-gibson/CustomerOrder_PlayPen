namespace CustomerOrder.PriceServiceStub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class SimplePriceGateway : IPrice
    {
        private readonly Dictionary<Tuple<Currency, ProductIdentifier>, QuantityPrice> _priceLookup;

        public SimplePriceGateway()
        {
            _priceLookup = new Dictionary<Tuple<Currency, ProductIdentifier>, QuantityPrice>();
            SetPrice("trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b", new QuantityPrice(new Money(Currency.GBP, 0.50m), 1));
        }

        public void SetPrice(ProductIdentifier productIdentifier, QuantityPrice price)
        {
            var tuple = new Tuple<Currency, ProductIdentifier>(price.Currency, productIdentifier);
            _priceLookup[tuple] = price;
        }

        public IPricedOrder Price(ICustomerOrder order)
        {
            var targetCurrency = order.Currency;
            var productPrices = order.Products.ToDictionary(product => product, product => PriceProduct(product, targetCurrency));
            return new PricedOrder(productPrices, targetCurrency);
        }

        private PricedProduct PriceProduct(IProduct product, Currency currency)
        {
            var quantityPrice = GetQuantityPrice(product, currency);
            var unitPrice = quantityPrice * new Quantity(1, UnitOfMeasure.Each);
            var netPrice = quantityPrice * product.Quantity;

            return new PricedProduct(product, unitPrice, netPrice);
        }

        private QuantityPrice GetQuantityPrice(IProduct product, Currency currency)
        {
            var tuple = new Tuple<Currency, ProductIdentifier>(currency, product.ProductIdentifier);

            try
            {
                return _priceLookup[tuple];
            }
            catch (Exception)
            {
                throw new ProductNotFoundException(product.ProductIdentifier);
            }
        }
    }
}
