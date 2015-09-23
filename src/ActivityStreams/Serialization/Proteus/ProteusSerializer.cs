using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cassandra;
using Elders.Proteus;
using ActivityStreams.Persistence.InMemory;

namespace ActivityStreams.Persistence.Cassandra
{

    public class ProteusSerializer : ISerializer
    {
        Elders.Proteus.Serializer serializer;

        public ProteusSerializer(Assembly[] assembliesContainingContracts)
        {
            var internalAssemblies = assembliesContainingContracts.ToList();
            internalAssemblies.Add(typeof(Elders.Proteus.Serializer).Assembly);

            var identifier = new GuidTypeIdentifier(internalAssemblies.ToArray());
            serializer = new Elders.Proteus.Serializer(identifier);
        }

        public object Deserialize(System.IO.Stream str)
        {
            return serializer.DeserializeWithHeaders(str);
        }

        public void Serialize<T>(System.IO.Stream str, T message)
        {
            serializer.SerializeWithHeaders(str, message);
        }
    }
}