using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.Cassandra;
using ActivityStreams.Persistence.InMemory;
using Machine.Specifications;

namespace ActivityStreams.Tests.Activities
{
    [Subject("Streams")]
    public class When_loading_multiple_activity_stream
    {
        Establish context = () =>
            {
                var streamId1 = Encoding.UTF8.GetBytes("streamId1");
                var streamId2 = Encoding.UTF8.GetBytes("streamId2");

                var id1 = Encoding.UTF8.GetBytes("activityId1");
                item1 = new Activity(streamId1, id1, "body1", "author1", DateTime.UtcNow.AddMinutes(1));

                var id2 = Encoding.UTF8.GetBytes("activityId2");
                item2 = new Activity(streamId2, id2, "body2", "author2", DateTime.UtcNow.AddMinutes(2));

                var id3 = Encoding.UTF8.GetBytes("activityId3");
                item3 = new Activity(streamId1, id3, "body3", "author3", DateTime.UtcNow.AddMinutes(3));

                activityStreamRepository = new InMemoryActivityRepository();
                activityStreamRepository.Append(item2);
                activityStreamRepository.Append(item3);
                activityStreamRepository.Append(item1);

                var subscriptionOwnerId = Encoding.UTF8.GetBytes("subscriptionOwnerId");
                var feedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));
                feed = feedFactory.Get(subscriptionOwnerId);
                feed.Attach(new Stream(subscriptionOwnerId, streamId1));
                feed.Attach(new Stream(subscriptionOwnerId, streamId2));

                paging = new Paging(DateTime.UtcNow.AddYears(3).ToFileTimeUtc(), int.MaxValue);
                sortOrder = SortOrder.Descending;
                feedOptions = new FeedOptions(paging, sortOrder);
            };

        Because of = () => activityStream = activityStreamRepository.Load(feed, feedOptions).ToList();

        It should_return_all_activities = () => activityStream.Count.ShouldEqual(3);

        It should_return_ordered_activity_stream_by_timestamp = () =>
        {
            activityStream[0].ShouldEqual(item1);
            activityStream[1].ShouldEqual(item2);
            activityStream[2].ShouldEqual(item3);
        };

        static IActivityRepository activityStreamRepository;
        static Feed feed;
        static List<Activity> activityStream;
        static Activity item1;
        static Activity item2;
        static Activity item3;
        static Paging paging;
        static SortOrder sortOrder;
        static FeedOptions feedOptions;
    }
}
