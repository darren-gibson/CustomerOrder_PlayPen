namespace CustomerOrder.Model.Command
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class CommandRunner
    {
        private readonly ICommandRepository _repository;
        private readonly ICommandQueue _queue;
        private int _activeThreadCount;

        public CommandRunner(ICommandRepository repository, ICommandQueue queue)
        {
            _repository = repository;
            _queue = queue;
            Status = CommandRunnerStatus.Stopped;
            Start();
        }

        public void Start()
        {
            if (Status == CommandRunnerStatus.Stopped)
            {
                Status = CommandRunnerStatus.Started;
                /*var workerThread = new Thread(DequeuAndExecuteCommands);
                workerThread.Start();*/
                Task.Factory.StartNew(DequeuAndExecuteCommands);
            }
        }

        public void Stop()
        {
            if (Status == CommandRunnerStatus.Started)
                Status = CommandRunnerStatus.Stopping;
        }

        public CommandRunnerStatus Status { get; private set; }
        public int ActiveWorkers { get { return _activeThreadCount; } }
        public event EventHandler<ICommand> CommandComplete;

        protected virtual void OnCommandComplete(ICommand e)
        {
            if (CommandComplete != null) 
                CommandComplete(this, e);
        }

        public void RunCommand(ICommand command)
        {
            EnsureValidCommand(command);
            _repository.Save(command);
            _queue.Enqueue(command);
        }

        private void EnsureValidCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Id == Guid.Empty)
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("command.Id", "Must supply a command identifier");
        }

        private void DequeuAndExecuteCommands()
        {
            IncrementThreadCount();
            while (Status == CommandRunnerStatus.Started)
            {
                ICommand command;
                if ((command = _queue.TryDequeue(100)) != null)
                {
                    var result = RunCommandAndReturnResultOrException(command);
                    // TODO: The command could complete successfully and the result may never get saved - needs to be fixed.
                    _repository.Save(command, result);
                    OnCommandComplete(command);
                }
            }
            DecrementThreadCount();
        }

        private void DecrementThreadCount()
        {
            if (Interlocked.Decrement(ref _activeThreadCount) == 0)
            {
                Status = CommandRunnerStatus.Stopped;
                Debug.WriteLine("{0} CommandRunner stopped queue: {1}", GetHashCode(), _queue.GetHashCode());
            }
        }

        private void IncrementThreadCount()
        {
            Interlocked.Increment(ref _activeThreadCount);
        }

        private object RunCommandAndReturnResultOrException(ICommand command)
        {
            try
            {
                return command.Execute();
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}