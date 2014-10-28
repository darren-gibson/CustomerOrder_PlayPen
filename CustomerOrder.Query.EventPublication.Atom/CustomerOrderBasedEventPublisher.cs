namespace CustomerOrder.Query.EventPublication.Atom
{
    using System;
    using DTO;
    using Model;

    public class CustomerOrderBasedEventPublisher
    {
        private readonly IAtomEventRepository _repository;

        public CustomerOrderBasedEventPublisher(EventRasingCustomerOrderFactory factory, IAtomEventRepository repository)
        {
            _repository = repository;
            factory.CustomerOrderMade += Factory_OnCustomerOrderMade;
        }

        private void Factory_OnCustomerOrderMade(object sender, CustomerOrderMadeEventArgs args)
        {
            var customerOrder = args.CustomerOrder;
            customerOrder.ProductAdded += CustomerOrderOnProductAdded;
            customerOrder.OrderPriced += CustomerOrderOnOrderPriced;
        }

        private void CustomerOrderOnOrderPriced(object sender, OrderPricedEventArgs orderPricedEventArgs)
        {
            var customerOrder = CustomerOrder(sender);
            var orderPricedDto = new OrderPricedEvent(Guid.NewGuid(), customerOrder, orderPricedEventArgs.PricedOrder);
            var syndicationItem = new CustomerOrderGeneratedEventSyndicationItem<OrderPricedEvent>(customerOrder, orderPricedDto);

            _repository.SaveEventToCurrentFeed(syndicationItem);
        }

        private void CustomerOrderOnProductAdded(object sender, ProductAddedEventArgs productAddedEventArgs)
        {
            var customerOrder = CustomerOrder(sender);
            var productAddedDto = new ProductAddedEvent(productAddedEventArgs.Id, customerOrder, productAddedEventArgs.ProductAdded);
            var productAddedSyndicationItem = new CustomerOrderGeneratedEventSyndicationItem<ProductAddedEvent>(customerOrder, productAddedDto);
            
            _repository.SaveEventToCurrentFeed(productAddedSyndicationItem);
        }

        private static ICustomerOrder CustomerOrder(object sender)
        {
            return (ICustomerOrder) sender;
        }
    }
}
