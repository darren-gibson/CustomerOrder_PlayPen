namespace CustomerOrder.Contract.ResultSerializer
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public class ResultSerializer<TModel, TContract> : IResultSerializer
    {
        public void Serialize(string contentType, object model, Stream outputStream) 
        {
            if (outputStream == null) throw new ArgumentNullException("outputStream");
            using (var streamWriter = new StreamWriter(outputStream))
            {
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    var objectToSerialize = (TContract)Activator.CreateInstance(typeof(TContract), new object[] { (TModel) model });

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonTextWriter, objectToSerialize);
                }
            }
        }

        public Type ContractType { get { return typeof (TContract); } }
    }
}
