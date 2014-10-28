namespace CustomerOrder.Contract.ResultSerializer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandResultSerializer
    {
        private readonly Dictionary<Type, List<IResultSerializer>> _commandSerializers;

        public CommandResultSerializer(IEnumerable<KeyValuePair<Type, IResultSerializer>> serializers)
        {
            _commandSerializers = new Dictionary<Type, List<IResultSerializer>>();

            foreach (var keyValuePair in serializers)
            {
                if (!_commandSerializers.ContainsKey(keyValuePair.Key))
                {
                    _commandSerializers.Add(keyValuePair.Key, new List<IResultSerializer>());
                }

                var serializerList = _commandSerializers[keyValuePair.Key];
                serializerList.Add(keyValuePair.Value);
            }
        }

        public IResultSerializer GetSerializerForResult(object result)
        {
            return _commandSerializers[result.GetType()].First();
        }
    }
}
