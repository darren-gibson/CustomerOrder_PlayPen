namespace CustomerOrder.Model.Command
{
    using System;

    public interface ICommandRepository
    {
        void Save(ICommand command, object result);
        void Save(ICommand command);
        object GetCommandResultById(Guid id);
    }
}
