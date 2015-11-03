using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using Cassandra;
using Machine.Specifications;

namespace ActivityStreams.Tests
{
    public abstract class InMemoryContext
    {
        Establish context = () =>
        {
            activityStore = new InMemoryActivityStore();
            activityStreamStore = new InMemoryStreamStore();

            activityRepository = new DefaultActivityRepository(activityStore, activityStreamStore);
            streamRepository = new DefaultStreamRepository(activityStreamStore);

            streamService = new StreamService(streamRepository);
        };

        protected static ISession session;
        protected static IActivityStore activityStore;
        protected static IStreamStore activityStreamStore;
        protected static IActivityRepository activityRepository;
        protected static IStreamRepository streamRepository;
        protected static StreamService streamService;
    }
}