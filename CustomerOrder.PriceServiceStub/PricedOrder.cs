namespace CustomerOrder.PriceServiceStub
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    internal class PricedOrder : IPricedOrder
    {
        private readonly Dictionary<IProduct, PricedProduct> _productPrices;
        private readonly Currency _baseCurrency;

        public PricedOrder(Dictionary<IProduct, PricedProduct> productPrices, Currency baseCurrency)
        {
            _productPrices = productPrices;
            _baseCurrency = baseCurrency;
        }

        public IProductPrice GetProductPrice(IProduct productEntryToGetPriceFor)
        {
            return _productPrices[productEntryToGetPriceFor];
        }

        public Money NetTotal
        {
            get
            {
                return _productPrices.Aggregate(new Money(_baseCurrency, 0), (current, pricedProduct) => current + pricedProduct.Value.NetPrice);
            }
        }
    }
}