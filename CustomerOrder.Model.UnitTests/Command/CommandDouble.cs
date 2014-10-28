namespace CustomerOrder.Model.UnitTests.Command
{
    using System;
    using System.Threading;
    using Model.Command;

    internal abstract class CommandDouble : ICommand
    {
        private readonly int _commandExecutionLength;

        protected CommandDouble(int commandExecutionLength)
        {
            _commandExecutionLength = commandExecutionLength;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public object Execute()
        {
            OnCommandStarted();
            Thread.Sleep(_commandExecutionLength);
            IsFinished = true;
            return "hello";
        }

        public event EventHandler CommandStarted;

        private void OnCommandStarted()
        {
            EventHandler handler = CommandStarted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public bool IsFinished { get; private set; }
    }
}