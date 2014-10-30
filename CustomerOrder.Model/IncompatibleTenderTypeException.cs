namespace CustomerOrder.Model
{
    using System;

    public class IncompatibleTenderTypeException : Exception
    {
        public IncompatibleTenderTypeException(string source, string target)
            : base(string.Format("{0} is incompatible with {1}.", source, target))
        {
            
        }
    }
}
