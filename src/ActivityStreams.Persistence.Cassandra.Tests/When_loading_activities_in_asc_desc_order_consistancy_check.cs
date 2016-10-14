//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Machine.Specifications;
//using ActivityStreams.Persistence.Cassandra.Tests.Helpers;
//using ActivityStreams.Persistence.Cassandra.Tests.Models;

//namespace ActivityStreams.Persistence.Cassandra.Tests
//{
//    [Subject("")]
//    public class When_loading_activities_in_asc_desc_order_consistancy_check
//    {
//        Establish context = () =>
//        {
//            var session = SessionCreator.Create();
//            var serializer = new ProteusSerializer(new[] { Assembly.GetAssembly(typeof(Activity)), Assembly.GetAssembly(typeof(ActivityBody)) });
//            var store = new ActivityStore(session, serializer);
//            activityRepository = new ActivityRepository(store);

//            ActivityStreamsStorageManager manager = new ActivityStreamsStorageManager(session);
//            manager.CreateActivitiesStorage();

//            var feedFactory = new StreamFactory(new StreamRepository(new FeedStreamStore(session)));
//            feed = new GGFactory(activityRepository, feedFactory).GetFeed(1, 5);
//            paging1 = new Paging(DateTime.UtcNow.AddYears(3).ToFileTimeUtc(), int.MaxValue);
//            sortOrder1 = SortOrder.Descending;
//            feedOptions1 = new FeedOptions(paging1, sortOrder1);

//            paging2 = new Paging(DateTime.UtcNow.AddYears(3).ToFileTimeUtc(), int.MaxValue);
//            sortOrder2 = SortOrder.Ascending;
//            feedOptions2 = new FeedOptions(paging2, sortOrder2);
//        };

//        Because of = () =>
//        {
//            activitiesDesc = activityRepository.Load(feed, feedOptions1).ToList();
//            activitiesAsc = activityRepository.Load(feed, feedOptions2).ToList();
//        };

//        It should_return_two_sequence_equal_enumerables = () =>
//        {
//            activitiesAsc.Reverse();
//            activitiesDesc.SequenceEqual(activitiesAsc).ShouldEqual(true);
//        };

//        static List<Activity> activitiesDesc;
//        static List<Activity> activitiesAsc;
//        static IStream feed;
//        static Paging paging1;
//        static SortOrder sortOrder1;
//        static FeedOptions feedOptions1;

//        static Paging paging2;
//        static SortOrder sortOrder2;
//        static FeedOptions feedOptions2;
//        static IActivityRepository activityRepository;
//    }
//}
