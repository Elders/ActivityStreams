using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Machine.Specifications;
using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
using ActivityStreams.Persistence.Cassandra.Tests.Models;

namespace ActivityStreams.Persistence.Cassandra.Tests
{
    [Subject("")]
    public class When_loading_from_multiple_activity_streams
    {
        Establish context = () =>
        {
            var session = SessionCreator.Create();
            var serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
            var store = new ActivityStore(session, serializer);
            activityRepository = new ActivityRepository(store);

            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
            manager.CreateActivitiesStorage();

            feed = new GGFactory(activityRepository).GetFeed(10, 3);
        };

        Because of = () => { results = activityRepository.Load(feed, DateTime.UtcNow.AddYears(2)).ToList(); };

        It should_do_the_right_job = () =>
        {
            results.Count.ShouldEqual(9);
        };

        static List<Activity> results;
        static Feed feed;
        static IActivityRepository activityRepository;
    }

    public class GGFactory
    {
        readonly IActivityRepository activityRepository;

        public GGFactory(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Feed GetFeed(int numberOfStreams, int activitiesPerStream)
        {
            var feedId = Encoding.UTF8.GetBytes("feedid");
            var feed = new Feed(feedId);
            for (int i = 0; i < numberOfStreams; i++)
            {
                for (int j = 0; j < activitiesPerStream; j++)
                {
                    var streamId = Guid.NewGuid();
                    var feedStream = new FeedStream(feedId, streamId.ToByteArray());
                    feed.AttachStream(feedStream);
                    var externalId = $"extid-{i}-{j}";
                    var body = new ActivityBody() { Content = externalId };

                    var activity = Activity.UnitTestFactory(streamId.ToByteArray(), Encoding.UTF8.GetBytes(externalId), body, "author", DateTime.UtcNow.AddYears(i));
                    activityRepository.Append(activity);
                }
            }

            return feed;
        }
    }
}
