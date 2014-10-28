namespace CustomerOrder.Contract.ResultSerializer
{
    using System.IO;

    public interface IResultSerializer
    {
        void Serialize(string contentType, object model, Stream outputStream);
    }
}