namespace CustomerOrder.Model
{
    using System;
    
    [Serializable]
    public class IncompatibleUnitOfMeasureException : Exception
    {
        public IncompatibleUnitOfMeasureException(UnitOfMeasure source, UnitOfMeasure target)
            : base(string.Format("Unable to convert UnitOfMeasure from {0} to {1}.", source, target))
        {
            
        }
    }
}