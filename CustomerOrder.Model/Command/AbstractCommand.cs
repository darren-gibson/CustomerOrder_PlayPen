namespace CustomerOrder.Model.Command
{
    using System;
    using System.Runtime.Serialization;

    public abstract class AbstractCommand : ICommand
    {
        protected AbstractCommand(Guid? id = null)
        {
            Id = id.HasValue ? id.Value : Guid.NewGuid();
        }
        public Guid Id { get; private set; }
        public abstract object Execute();
    }
}