namespace CustomerOrder.Model.Repository
{
    using System;
    using System.Collections.Concurrent;
    using Command;

    public class InMemoryCommandRepository : ICommandRepository
    {
        private ConcurrentDictionary<Guid, object> _repository;

        public InMemoryCommandRepository()
        {
            _repository = new ConcurrentDictionary<Guid, object>();
        }
        public void Save(ICommand command, object result)
        {
            if(result == null)
                throw new ArgumentNullException("result");
            _repository[command.Id] = result;
        }

        public void Save(ICommand command)
        {
            _repository[command.Id] = null;
        }

        public object GetCommandResultById(Guid id)
        {
            object result;
            if (_repository.TryGetValue(id, out result))
                return result;
            throw new NotFoundException();
        }
    }

    public class NotFoundException : Exception
    {
    }
}
