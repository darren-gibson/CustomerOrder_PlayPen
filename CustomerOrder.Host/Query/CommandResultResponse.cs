namespace CustomerOrder.Host.Query
{
    using Contract.ResultSerializer;
    using Nancy;

    class CommandResultResponse : Response
    {
        public CommandResultResponse(IResultSerializer serializer, object result)
        {
            Contents = stream => serializer.Serialize(ContentType, result, stream);
            ContentType = string.Format("application/vnd.tesco.CustomerOrder.{0}+json", serializer.ContractType.Name);
        }
    }
}
