namespace CustomerOrder.Model.Command
{
    using System;
    using System.Runtime.Serialization;

    public interface ICommand
    {
        Guid Id { get; }
        object Execute();
    }
}
