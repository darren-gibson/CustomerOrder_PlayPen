namespace CustomerOrder.Host
{
    using Annotations;
    using Nancy;
    using ProductServiceStub;

    [UsedImplicitly]
    public class ProductServiceModule : NancyModule
    {
        public ProductServiceModule(SimpleProductService productService)
            : base("/v2")
        {
            Get["/products"] = parameters =>
            {
                var products = ((string) Request.Query.gtin).Split(',');
                return new Response
                {
                    Contents = stream => productService.Serialize(products, stream),
                };
            };
        }
    }
}