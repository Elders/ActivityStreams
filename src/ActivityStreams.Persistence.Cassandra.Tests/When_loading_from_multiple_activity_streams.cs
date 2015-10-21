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

            var feedFactory = new FeedFactory(new FeedStreamRepository(new FeedStreamStore(session)));
            feed = new GGFactory(activityRepository, feedFactory).GetFeed(10, 2);
            paging = new Paging(DateTime.UtcNow.AddYears(3).ToFileTimeUtc(), 20);
            sortOrder = SortOrder.Descending;
            feedOptions = new FeedOptions(paging, sortOrder);
        };

        Because of = () => { results = activityRepository.Load(feed, feedOptions).ToList(); };

        It should_do_the_right_job = () =>
        {
            results.Count.ShouldEqual(8);
        };

        static List<Activity> results;
        static Feed feed;
        static Paging paging;
        static SortOrder sortOrder;
        static FeedOptions feedOptions;
        static IActivityRepository activityRepository;
    }

    public class GGFactory
    {
        readonly IActivityRepository activityRepository;
        readonly FeedFactory feedFactory;

        public GGFactory(IActivityRepository activityRepository, FeedFactory feedFactory)
        {
            this.activityRepository = activityRepository;
            this.feedFactory = feedFactory;
        }

        public Feed GetFeed(int numberOfStreams, int activitiesPerStream)
        {
            var feedId = Encoding.UTF8.GetBytes("feedid" + DateTime.UtcNow.ToFileTimeUtc());
            var feed = feedFactory.GG(feedId);
            for (int i = 0; i < numberOfStreams; i++)
            {
                for (int j = 0; j < activitiesPerStream; j++)
                {
                    var streamId = Guid.NewGuid();
                    var feedStream = new Stream(feedId, streamId.ToByteArray());
                    feed.Attach(feedStream);
                    var externalId = $"extid-{i}-{j}";
                    var body = new ActivityBody() { Content = externalId };

                    var activity = new Activity(streamId.ToByteArray(), Encoding.UTF8.GetBytes(externalId), body, "author", DateTime.UtcNow.AddYears(i));
                    activityRepository.Append(activity);
                }
            }

            return feed;
        }
    }
}
