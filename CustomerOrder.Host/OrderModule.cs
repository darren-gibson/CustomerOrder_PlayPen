namespace CustomerOrder.Host
{
    using Contract.DTO;
    using Model;
    using Model.Command;
    using Model.Repository;
    using Nancy;
    using Nancy.ModelBinding;
    using CustomerOrder = Contract.DTO.CustomerOrder;

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
            Put["/{orderId}/payments"] = parameters =>
            {
                var paymentAdd = this.Bind<PaymentAdd>();
                var command = new PaymentAddCommand(_repository, paymentAdd.OrderId, new Tender(paymentAdd.Amount.ToMoney(), paymentAdd.TenderType));
                return RunCommand(command, Context, parameters.orderId);
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
            return new ProductAddCommand(_repository, productAdd.OrderId, productAdd.ProductId, productAdd.Quantity.ToQuantity());
        }
    }
}