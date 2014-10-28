namespace CustomerOrder.Host
{
    using Annotations;
    using Contract.ResultSerializer;
    using Model.Command;
    using Nancy;
    using Query;

    [UsedImplicitly]
    public class CommandModule : NancyModule
    {
        private readonly ICommandRepository _repository;
        private readonly CommandResultSerializer _resultSerializer;

        public CommandModule(ICommandRepository repository, CommandResultSerializer resultSerializer)
            : base("/orders")
        {
            _repository = repository;
            _resultSerializer = resultSerializer;
            Get["/{orderId}/requests/{commandId}"] = parameters =>
            {
                var result = _repository.GetCommandResultById(parameters.commandId);
                if (result == null)
                    return new NullCommandResultResponse();
                return new CommandResultResponse(_resultSerializer.GetSerializerForResult(result), result);
            };
        }
    }
}