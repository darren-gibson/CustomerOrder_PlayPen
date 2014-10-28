namespace CustomerOrder.Model.Command
{
    public interface ICommandQueue
    {
        void Enqueue(ICommand command);
        ICommand TryDequeue(long timeoutInMilliseconds = 0);
    }
}