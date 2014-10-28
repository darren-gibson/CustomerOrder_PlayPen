namespace CustomerOrder.PriceServiceStub
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    internal class PricedOrder : IPricedOrder
    {
        private readonly Dictionary<IProduct, PricedProduct> _productPrices;

        public PricedOrder(Dictionary<IProduct, PricedProduct> productPrices)
        {
            _productPrices = productPrices;
        }

        public IProductPrice GetProductPrice(IProduct productEntryToGetPriceFor)
        {
            return _productPrices[productEntryToGetPriceFor];
        }

        public Money NetTotal
        {
            get
            {
                if(_productPrices.Count == 0)
                    return new Money(Currency.GBP, 0);  // TODO: Where does the currency code come from?
                var result = new Money(_productPrices.First().Value.NetPrice.Code, 0m);
                return _productPrices.Aggregate(result, (current, pricedProduct) => current + pricedProduct.Value.NetPrice);
            }
        }
    }
}