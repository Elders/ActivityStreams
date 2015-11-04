using System.Reflection;
using Cassandra;
using Machine.Specifications;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;

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
        };

        protected static ISession session;
        protected static ISerializer serializer;
        protected static IActivityStore activityStore;
        protected static IStreamStore activityStreamStore;
        protected static IActivityRepository activityRepository;
        protected static IStreamRepository streamRepository;
        protected static StreamService streamService;
    }
}