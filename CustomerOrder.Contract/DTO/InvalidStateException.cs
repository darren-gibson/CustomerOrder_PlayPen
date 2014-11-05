namespace CustomerOrder.Contract.DTO
{
    using System;

    public class InvalidStateException
    {
        public InvalidStateException(InvalidOperationException exception)
        {
        }
    }
}
