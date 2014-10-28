namespace CustomerOrder.Host
{
    using Annotations;
    using Contract.DTO;
    using Model.Command;
    using Model.Repository;
    using Nancy;
    using Nancy.ModelBinding;
    using Quantity = Model.Quantity;

    public class OrderModule : NancyModule
    {
        private readonly ICustomerOrderRepository _repository;
        private readonly CommandRunner _runner;

        public OrderModule(ICustomerOrderRepository repository, CommandRunner runner) : base("/orders")
        {
            _repository = repository;
            _runner = runner;
            Post["/{orderId}/productAdd"] = parameters =>
            {
                var command = CreateProductAddedCommand(this.Bind<ProductAdd>());
                return RunCommand(command, Context, parameters.orderId);
            };
            Get["/{orderId}/"] = parameters =>
            {
                var order = repository.GetOrCreateOrderById((string)parameters["orderId"]);
                return new CustomerOrder(order);
            };
        }

        private Response RunCommand(ICommand command, NancyContext context, string orderId)
        {
            _runner.RunCommand(command);

            return new Response
            {
                Headers = { { "Location", BuildLocation(context, command, orderId) }},
                StatusCode = HttpStatusCode.Accepted
            };

        }

        private string BuildLocation(NancyContext context, ICommand command, string orderId)
        {
            return context.Request.Url.SiteBase + 
                string.Format("/orders/{0}/requests/{1}", orderId, command.Id);
        }

        private ICommand CreateProductAddedCommand(ProductAdd productAdd)
        {
            if (productAdd.Quantity == null)
                return new ProductAddCommand(_repository, productAdd.OrderId, productAdd.ProductId);
            return new ProductAddCommand(_repository, productAdd.OrderId, productAdd.ProductId, ConvertToQuantity(productAdd));
        }

        private static Quantity ConvertToQuantity(ProductAdd productAdd)
        {
            return new Quantity(productAdd.Quantity.Amount, productAdd.Quantity.UOM);
        }
    }
}