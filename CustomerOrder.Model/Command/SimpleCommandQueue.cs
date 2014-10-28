namespace CustomerOrder.Model.Command
{
    using System;
    using System.Collections.Concurrent;

    public class SimpleCommandQueue : ICommandQueue
    {
        private BlockingCollection<ICommand> _internalQueue;

        public SimpleCommandQueue()
        {
            _internalQueue = new BlockingCollection<ICommand>(new ConcurrentQueue<ICommand>());
        }

        public int Length { get { return _internalQueue.Count; } }

        public void Enqueue(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            _internalQueue.Add(command);
        }

        public ICommand TryDequeue(long timeoutInMilliseconds = 0)
        {
            ICommand result;
            _internalQueue.TryTake(out result, (int)timeoutInMilliseconds);
            return result;
        }
    }
}