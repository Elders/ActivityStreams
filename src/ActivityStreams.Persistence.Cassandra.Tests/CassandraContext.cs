using System.Reflection;
using Cassandra;
using Machine.Specifications;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;
using System;
using System.Linq;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    public abstract class CassandraContext
    {
        Establish context = () =>
        {
            session = SessionCreator.Create();
            serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });

            StorageManager manager = new StorageManager(session);
            manager.CreateActivitiesStorage();
            manager.CreateStreamsStorage();

            activityStore = new ActivityStore(session, serializer);
            activityStreamStore = new StreamStore(session);

            activityRepository = new DefaultActivityRepository(activityStore, activityStreamStore);
            streamRepository = new DefaultStreamRepository(activityStreamStore);

            streamService = new StreamService(streamRepository);

            sandbox = DateTime.UtcNow.ToFileTimeUtc().ToString();
        };

        protected static ISession session;
        protected static ISerializer serializer;
        protected static IActivityStore activityStore;
        protected static IStreamStore activityStreamStore;
        protected static IActivityRepository activityRepository;
        protected static IStreamRepository streamRepository;
        protected static StreamService streamService;
        protected static string sandbox;
    }

    public class ProteusSerializer : ISerializer
    {
        Elders.Proteus.Serializer serializer;

        public ProteusSerializer(Assembly[] assembliesContainingContracts)
        {
            var internalAssemblies = assembliesContainingContracts.ToList();
            internalAssemblies.Add(typeof(Elders.Proteus.Serializer).Assembly);

            var identifier = new Elders.Proteus.GuidTypeIdentifier(internalAssemblies.ToArray());
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
