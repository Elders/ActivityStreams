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
    public class When_loading_activities_in_asc_desc_order_consistancy_check
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
            feed = new GGFactory(activityRepository, feedFactory).GetFeed(1, 5);
            paging = new Paging(DateTime.UtcNow.AddYears(3).ToFileTimeUtc(), int.MaxValue);
        };

        Because of = () =>
        {
            activitiesDesc = activityRepository.Load(feed, paging, SortOrder.Descending).ToList();
            activitiesAsc = activityRepository.Load(feed, paging, SortOrder.Ascending).ToList();
        };

        It should_return_two_sequence_equal_enumerables = () =>
        {
            activitiesAsc.Reverse();
            activitiesDesc.SequenceEqual(activitiesAsc).ShouldEqual(true);
        };

        static List<Activity> activitiesDesc;
        static List<Activity> activitiesAsc;
        static Feed feed;
        static Paging paging;
        static IActivityRepository activityRepository;
    }
}
