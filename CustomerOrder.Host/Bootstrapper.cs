namespace CustomerOrder.Host
{
    using System;
    using System.Collections.Generic;
    using Contract.ResultSerializer;
    using CustomerOrder.Query.EventPublication.Atom;
    using Model;
    using Model.Command;
    using Model.Repository;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;
    using Newtonsoft.Json;
    using PriceServiceStub;
    using ProductServiceStub;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var commandRepository = new InMemoryCommandRepository();
            var runner = new CommandRunner(commandRepository, new SimpleCommandQueue());
            var priceGateway = new SimplePriceGateway();
            var factory = new EventRasingCustomerOrderFactory(new CustomerOrderFactory(priceGateway));
            var customerOrderRepository = new InMemoryCustomerOrderRepository(factory);
            var atomEventRepository = new SimpleInMemoryAtomEventRepository();
            var commandResultSerializer = GetResultSerializers();
            var simpleProductService = new SimpleProductService();
            container.Register(new CustomerOrderBasedEventPublisher(factory, atomEventRepository));
            container.Register(runner);
            container.Register<ICustomerOrderRepository>(customerOrderRepository);
            container.Register(commandResultSerializer);
            container.Register<ICommandRepository>(commandRepository);
            container.Register<IAtomEventRepository>(atomEventRepository);
            container.Register(priceGateway);
            container.Register(simpleProductService);

            Registry = new Registry(container);
            base.ApplicationStartup(container, pipelines);
        }

        private static CommandResultSerializer GetResultSerializers()
        {
            return new CommandResultSerializer(new[]
                {
                    new KeyValuePair<Type, IResultSerializer>(typeof (ProductAdded), new ResultSerializer<ProductAdded, Contract.DTO.ProductAdded>()),
                    new KeyValuePair<Type, IResultSerializer>(typeof (ProductNotFoundException), new ResultSerializer<ProductNotFoundException, Contract.DTO.ProductNotFoundException>()),
                    new KeyValuePair<Type, IResultSerializer>(typeof (PaymentAdded), new ResultSerializer<PaymentAdded, Contract.DTO.PaymentAdded>()),
                });
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type, X-Requested-With")
                .WithHeader("Access-Control-Expose-Headers", "Location"));
            base.RequestStartup(container, pipelines, context);
        }

        public Registry Registry { get; private set; }
    }
}