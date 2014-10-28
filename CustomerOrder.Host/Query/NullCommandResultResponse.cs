namespace CustomerOrder.Host.Query
{
    using Nancy;

    public class NullCommandResultResponse : Response
    {
        public NullCommandResultResponse()
        {
            ContentType = "application/vnd.tesco.CustomerOrder.NotComplete+json";
        }
    }
}
