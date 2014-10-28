namespace CustomerOrder.Host
{
    using Nancy.TinyIoc;

    public class Registry
    {
        private readonly TinyIoCContainer _container;

        public Registry(TinyIoCContainer container)
        {
            _container = container;
        }

        public T Resolve<T>() where T : class 
        {
            return _container.Resolve<T>();
        }
    }
}
