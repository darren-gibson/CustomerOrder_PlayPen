namespace CustomerOrder.Contract.DTO
{
    public class ProductNotFoundException
    {
        private readonly Model.ProductNotFoundException _exception;

        public ProductNotFoundException(Model.ProductNotFoundException exception)
        {
            _exception = exception;
        }
    }
}
