namespace CustomerOrder.PriceServiceStub
{
    using Model;

    internal class PricedProduct : IProductPrice
    {
        public PricedProduct(IProduct product, Money unitPrice, Money netPrice)
        {
            Product = product;
            UnitPrice = unitPrice;
            NetPrice = netPrice;
        }

        public IProduct Product { get; private set; }

        public Money UnitPrice { get; private set; }
        public Money NetPrice{ get; private set; }
    }
}
